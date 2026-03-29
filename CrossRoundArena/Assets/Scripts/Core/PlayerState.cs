using System.Collections.Generic;
using CrossRoundArena.Data;
using CrossRoundArena.Core;

namespace CrossRoundArena.Core
{
    [System.Serializable]
    public class PlayerState : IBattleTarget
    {
        public int id;
        public string playerName;
        public LeaderData leader;
        public int currentHP;
        public int maxHP;
        public int currentCost;
        public int maxCost;
        public int coins;
        public int ghostReviveCount = 0;
        public int ghostTurnCounter = 0;
        public PlayerStatus status = PlayerStatus.Alive;

        public List<CardData> deck = new List<CardData>();
        public List<CardData> hand = new List<CardData>();
        public List<MonsterInstance> boardMonsters = new List<MonsterInstance>();

        public int CurrentHP => currentHP;
        public bool IsDead => currentHP <= 0;

        public void TakeDamage(int damage, DamageSource source)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (ghostReviveCount < 2)
            {
                status = PlayerStatus.Ghost;
                ghostTurnCounter = 3;
                currentHP = 0; // 幽霊状態
            }
            else
            {
                // ゲームから完全脱落
            }
        }

        public void RecoverHP(int amount)
        {
            currentHP = UnityEngine.Mathf.Min(currentHP + amount, maxHP);
        }

        public void HandleGhostTurn()
        {
            if (status == PlayerStatus.Ghost)
            {
                ghostTurnCounter--;
                if (ghostTurnCounter <= 0)
                {
                    Revive();
                }
            }
        }

        public void Revive()
        {
            status = PlayerStatus.Alive;
            currentHP = 2;
            ghostReviveCount++;
        }

        public void DrawCard(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                if (deck.Count > 0)
                {
                    CardData card = deck[0];
                    hand.Add(card);
                    deck.RemoveAt(0);
                }
                else
                {
                    // デッキ切れ処理（シャッフルや墓地リセなど）
                    break;
                }
            }
        }

        public void ShuffleDeck()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                CardData temp = deck[i];
                int randomIndex = UnityEngine.Random.Range(i, deck.Count);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }

        public void AddToHand(CardData card)
        {
            hand.Add(card);
        }

        // Leader attack condition check
        public bool IsLeaderTargetable()
        {
            // Condition 1: Opponent monsters <= 4 (according to the planning doc "相手モンスター ≤ 4体")
            if (boardMonsters.Count > 4) return false;

            // Condition 2: No monsters with Guard keyword (ガード持ちがいない)
            foreach (var monster in boardMonsters)
            {
                if (monster.HasKeyword(Keyword.Guard)) return false;
            }

            return true;
        }

        public bool HasGuardOnBoard()
        {
            foreach (var monster in boardMonsters)
            {
                if (monster.HasKeyword(Keyword.Guard)) return true;
            }
            return false;
        }

        public void SummonMonster(MonsterCardData data)
        {
            if (boardMonsters.Count < 5)
            {
                var newMonster = new MonsterInstance(data, this);
                boardMonsters.Add(newMonster);

                // もし「同名カード強化」持ちがいれば、盤面全体の攻撃力を再計算する
                foreach (var monster in boardMonsters)
                {
                    monster.UpdatePower();
                }

                // Speed calculation if needed
                if (newMonster.HasKeyword(Keyword.Haste))
                {
                    newMonster.RefreshTurn();
                }
            }
        }
    }
}
