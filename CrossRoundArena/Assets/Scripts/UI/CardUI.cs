using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CrossRoundArena.Data;
using CrossRoundArena.Core; // Added missing namespace

namespace CrossRoundArena.UI
{
    public class CardUI : MonoBehaviour
    {
        [JapaneseLabel("カード名テキスト")] public TextMeshProUGUI cardNameText;
        [JapaneseLabel("説明テキスト")] public TextMeshProUGUI cardDescriptionText;
        [JapaneseLabel("コストテキスト")] public TextMeshProUGUI costText;
        [JapaneseLabel("カード画像")] public Image cardIcon;
        [JapaneseLabel("背景画像")] public Image cardBackground;

        [JapaneseLabel("カードデータ")] public CardData cardData;

        public void SetCard(CardData card)
        {
            cardData = card;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (cardData == null) return;

            cardNameText.text = cardData.cardName;
            cardDescriptionText.text = cardData.cardDescription;
            costText.text = cardData.initialCost.ToString();
            
            if (cardData.cardIcon != null)
                cardIcon.sprite = cardData.cardIcon;

            if (cardData is MonsterCardData monsterData)
            {
                // ATK/HP display logic could go here
            }
        }

        public void OnClick()
        {
            if (cardData == null) return;

            // GameManager, CardPlaySystem, TargetSelectionManager are all in CrossRoundArena.Core
            if (GameManager.instance == null) return;

            var activePlayer = GameManager.instance.activePlayers[GameManager.instance.currentPlayerIndex];

            string reason;
            if (!CardPlaySystem.CanPlayCard(activePlayer, cardData, out reason))
            {
                Debug.LogWarning($"Cannot play card: {reason}");
                return;
            }

            if (cardData is SkillCardData skillData && skillData.targetType != TargetType.None)
            {
                if (TargetSelectionManager.instance != null)
                {
                    TargetSelectionManager.instance.StartSkillTargetSelection(skillData, target => 
                    {
                        CardPlaySystem.PlayCard(activePlayer, skillData, target);
                        UpdateUI();
                    });
                }
            }
            else if (cardData is EquipmentCardData equipData)
            {
                if (TargetSelectionManager.instance != null)
                {
                    TargetSelectionManager.instance.StartEquipmentTargetSelection(equipData, target => 
                    {
                        CardPlaySystem.PlayCard(activePlayer, equipData, target);
                        UpdateUI();
                    });
                }
            }
            else
            {
                CardPlaySystem.PlayCard(activePlayer, cardData);
                UpdateUI();
            }
        }
    }
}
