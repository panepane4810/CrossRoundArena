using UnityEngine;
using CrossRoundArena.Core;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "SkillCard", menuName = "ScriptableObjects/CardData/SkillCard")]
    public class SkillCardData : CardData
    {
        [JapaneseLabel("スキルタイプ")] public SkillType skillType;
        [JapaneseLabel("ターゲットタイプ")] public TargetType targetType;
        [JapaneseLabel("効果量")] public int effectValue;
        [JapaneseLabel("魔法コスト")] public int manaCost;
        
        private void OnEnable()
        {
            cardType = CardType.Skill;
        }
    }
}
