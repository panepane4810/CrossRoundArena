using UnityEngine;
using CrossRoundArena.Core;
using System.Collections.Generic;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData/BaseCard", order = 2)]
    public class CardData : ScriptableObject
    {
        [JapaneseLabel("カードタイプ")] public CardType cardType;
        [JapaneseLabel("カード名")] public string cardName;
        [TextArea] public string cardDescription;
        [JapaneseLabel("コスト")] public int initialCost;
        [JapaneseLabel("購入額")] public int purchaseCoin;
        [JapaneseLabel("カードアイコン")] public Sprite cardIcon;
        public List<Keyword> keywords = new List<Keyword>();
    }

    public enum SkillType
    {
        DirectDamage,
        Heal,
        DrawCard,
        GainCost,
        AoEDamage,
        Buff,
        Debuff
    }

    public enum TargetType
    {
        None,
        SingleMonster,
        SingleLeader,
        AnySingle,
        AllOpponentMonsters,
        AllAllyMonsters,
        AllPlayers
    }
}
