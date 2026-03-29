using UnityEngine;
using CrossRoundArena.Core;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "EquipmentCard", menuName = "ScriptableObjects/CardData/EquipmentCard")]
    public class EquipmentCardData : CardData
    {
        [JapaneseLabel("攻撃ボーナス")] public int attackBonus;
        [JapaneseLabel("HPボーナス")] public int hpBonus;

        private void OnEnable()
        {
            cardType = CardType.Equipment;
        }
    }
}
