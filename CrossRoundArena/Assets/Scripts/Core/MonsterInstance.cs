using CrossRoundArena.Data;
using System.Collections.Generic;

namespace CrossRoundArena.Core
{
    public class MonsterInstance : IBattleTarget
    {
        public MonsterCardData Data { get; private set; }
        public int currentAttack;
        public int currentHP;
        public int maxHP;
        public bool hasAttackedThisTurn = false;
        public List<Keyword> activeKeywords = new List<Keyword>();
        public PlayerState owner;

        public int CurrentHP => currentHP;
        public bool IsDead => currentHP <= 0;

        public MonsterInstance(MonsterCardData data, PlayerState owner)
        {
            this.Data = data;
            this.owner = owner;
            this.currentAttack = data.attack;
            this.maxHP = data.hp;
            this.currentHP = data.hp;
            this.activeKeywords.AddRange(data.keywords);
            
            UpdatePower();
            
            // "Haste" allows attacking immediately
            if (activeKeywords.Contains(Keyword.Haste))
            {
                hasAttackedThisTurn = false;
            }
            else
            {
                hasAttackedThisTurn = true; // Wait for one turn usually, but based on rules "Haste" might be default or special.
            }
        }

        public void TakeDamage(int amount, DamageSource source)
        {
            currentHP -= amount;
            if (currentHP < 0) currentHP = 0;
            
            // TODO: Death logic like triggering "OnDeath" effects
        }

        public void Attack(IBattleTarget target)
        {
            if (hasAttackedThisTurn) return;

            // Attack logic
            target.TakeDamage(currentAttack, DamageSource.MonsterAttack);

            // Counter-attack logic (only if target is another monster)
            if (target is MonsterInstance targetMonster)
            {
                this.TakeDamage(targetMonster.currentAttack, DamageSource.MonsterAttack);
                
                // Poison effect
                if (this.activeKeywords.Contains(Keyword.Poison))
                {
                    // Apply poison logic
                }

                // Lifesteal effect
                if (this.activeKeywords.Contains(Keyword.Lifesteal))
                {
                    owner.RecoverHP(currentAttack / 2); // Simple implementation
                }
            }

            hasAttackedThisTurn = true;
        }

        public void RefreshTurn()
        {
            hasAttackedThisTurn = false;
            UpdatePower();
        }

        public void UpdatePower()
        {
            if (Data == null || owner == null) return;

            int baseAttack = Data.attack;
            
            // 「同名カード強化」フラグがある場合のみ計算
            if (Data.buffBySameNameCount)
            {
                int sameNameCount = 0;
                foreach (var monster in owner.boardMonsters)
                {
                    // 自身も含めてカウント
                    if (monster.Data != null && monster.Data.cardName == this.Data.cardName)
                    {
                        sameNameCount++;
                    }
                }
                
                // 自分以外の同名カードの数だけ加算 (自身が1体なら+0, 2体なら+1...)
                // ※解釈により「自身の数だけ」なら sameNameCount そのまま
                currentAttack = baseAttack + (sameNameCount - 1);
            }
            else
            {
                // バフがない場合は基本値に戻す（他のバフ効果がある場合はここを調整）
                currentAttack = baseAttack;
            }
        }

        public bool HasKeyword(Keyword keyword)
        {
            return activeKeywords.Contains(keyword);
        }
    }
}
