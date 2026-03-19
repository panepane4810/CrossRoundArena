using UnityEngine;
using CrossRoundArena.Data;

namespace CrossRoundArena.Core
{
    public static class CardPlaySystem
    {
        public static bool CanPlayCard(PlayerState player, CardData card, out string reason)
        {
            reason = "";

            if (player == null || card == null)
            {
                reason = "Player or Card is null.";
                return false;
            }

            // 1. コストチェック
            if (player.currentCost < card.initialCost)
            {
                reason = "Not enough cost.";
                return false;
            }

            // 2. カードタイプ別の制限
            if (card is MonsterCardData)
            {
                if (player.boardMonsters.Count >= 5)
                {
                    reason = "Board is full (max 5 monsters).";
                    return false;
                }
            }
            else if (card is EquipmentCardData)
            {
                if (player.boardMonsters.Count == 0)
                {
                    reason = "No monsters to equip.";
                    return false;
                }
            }

            return true;
        }

        public static void PlayCard(PlayerState player, CardData card, IBattleTarget target = null)
        {
            string reason;
            if (!CanPlayCard(player, card, out reason))
            {
                Debug.LogWarning($"Cannot play card: {reason}");
                return;
            }

            // コスト消費
            player.currentCost -= card.initialCost;

            // 手札から削除
            player.hand.Remove(card);

            // 効果の発動
            if (card is MonsterCardData monsterData)
            {
                player.SummonMonster(monsterData);
            }
            else if (card is SkillCardData skillData)
            {
                ResolveSkill(player, skillData, target);
            }
            else if (card is EquipmentCardData equipData)
            {
                if (target is MonsterInstance monster)
                {
                    ApplyEquipment(monster, equipData);
                }
            }

            Debug.Log($"Played {card.cardName}. Remaining Cost: {player.currentCost}");
        }

        private static void ResolveSkill(PlayerState caster, SkillCardData skill, IBattleTarget target)
        {
            Debug.Log($"Resolving Skill: {skill.cardName} of type {skill.skillType}");
            
            switch (skill.skillType)
            {
                case SkillType.DirectDamage:
                    if (target != null)
                    {
                        target.TakeDamage(skill.effectValue, DamageSource.Skill);
                    }
                    break;

                case SkillType.Heal:
                    if (target is PlayerState player)
                    {
                        player.RecoverHP(skill.effectValue);
                    }
                    else if (target is MonsterInstance monster)
                    {
                        monster.currentHP = Mathf.Min(monster.currentHP + skill.effectValue, monster.maxHP);
                    }
                    else if (target == null)
                    {
                        // ターゲット指定なしなら自分を回復
                        caster.RecoverHP(skill.effectValue);
                    }
                    break;

                case SkillType.DrawCard:
                    caster.DrawCard(1); // Skill can define how many cards
                    break;

                case SkillType.GainCost:
                    caster.currentCost += skill.effectValue;
                    break;

                case SkillType.AoEDamage:
                    // 敵プレイヤー全員の盤面にダメージ（簡易実装として現ターンのプレイヤー以外）
                    foreach (var p in GameManager.instance.activePlayers)
                    {
                        if (p != caster)
                        {
                            // 各プレイヤーの場の全モンスターにダメージ
                            foreach (var m in new System.Collections.Generic.List<MonsterInstance>(p.boardMonsters))
                            {
                                m.TakeDamage(skill.effectValue, DamageSource.Skill);
                            }
                            // 死亡チェック
                            p.boardMonsters.RemoveAll(m => m.IsDead);
                        }
                    }
                    break;

                case SkillType.Buff:
                    if (target is MonsterInstance targetMonster)
                    {
                        targetMonster.currentAttack += skill.effectValue;
                    }
                    break;
            }
        }

        private static void ApplyEquipment(MonsterInstance monster, EquipmentCardData equipment)
        {
            monster.currentAttack += equipment.attackBonus;
            monster.maxHP += equipment.hpBonus;
            monster.currentHP += equipment.hpBonus; // 最大HP増加分、現在値も増やす
            
            Debug.Log($"Equipped {equipment.cardName} to {monster.Data.cardName}. New ATK: {monster.currentAttack}, HP: {monster.currentHP}");
        }
    }
}
