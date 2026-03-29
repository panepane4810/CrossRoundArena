using UnityEngine;
using System.Collections.Generic;
using CrossRoundArena.Core;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "MonsterCard", menuName = "ScriptableObjects/CardData/MonsterCard")]
    public class MonsterCardData : CardData
    {
        [JapaneseLabel("攻撃力")] public int attack;
        [JapaneseLabel("体力")] public int hp;
        [JapaneseLabel("速攻")] public bool hasHaste;
        [JapaneseLabel("ガード")] public bool hasGuard;
        [JapaneseLabel("同名カード強化")] public bool buffBySameNameCount;

        private void OnEnable()
        {
            cardType = CardType.Monster;
        }
    }
}
