using UnityEngine;
using CrossRoundArena.Core;
using System.Collections.Generic;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData/BaseCard", order = 2)]
    public class CardData : ScriptableObject
    {
        public CardType cardType;
        public string cardName;
        public string cardDescription;
        public int initialCost;
        public int purchaseCoin;
        public Sprite cardIcon;
        public List<Keyword> keywords = new List<Keyword>();
    }

    [CreateAssetMenu(fileName = "MonsterCard", menuName = "ScriptableObjects/CardData/MonsterCard")]
    public class MonsterCardData : CardData
    {
        public int attack;
        public int hp;
        public bool hasHaste;
        public bool hasGuard;

        private void OnEnable()
        {
            cardType = CardType.Monster;
        }
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

    [CreateAssetMenu(fileName = "SkillCard", menuName = "ScriptableObjects/CardData/SkillCard")]
    public class SkillCardData : CardData
    {
        public SkillType skillType;
        public TargetType targetType;
        public int effectValue;
        
        private void OnEnable()
        {
            cardType = CardType.Skill;
        }
    }

    [CreateAssetMenu(fileName = "EquipmentCard", menuName = "ScriptableObjects/CardData/EquipmentCard")]
    public class EquipmentCardData : CardData
    {
        public int attackBonus;
        public int hpBonus;
        // 装備ごとの付加能力をここに

        private void OnEnable()
        {
            cardType = CardType.Equipment;
        }
    }
}
