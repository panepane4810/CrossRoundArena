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
        [JapaneseLabel("カード画像(描画先)")] public Image cardImage;
        [JapaneseLabel("カード画像(ソース)")] public Sprite cardIcon;
        [JapaneseLabel("背景画像")] public Image cardBackground;
        [JapaneseLabel("攻撃力テキスト")] public TextMeshProUGUI attackText;
        [JapaneseLabel("体力テキスト")] public TextMeshProUGUI hpText;

        private void Awake()
        {
            if (cardImage == null) cardImage = GetComponent<Image>();
        }

        [JapaneseLabel("カードデータ")] public CardData cardData;

        private void Start()
        {
            UpdateUI();
        }

        public void SetCard(CardData card)
        {
            cardData = card;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (cardData == null) return;

            if (cardNameText != null) cardNameText.text = cardData.cardName;
            if (cardDescriptionText != null) cardDescriptionText.text = cardData.cardDescription;
            if (costText != null) costText.text = cardData.initialCost.ToString();
            
            if (cardData.cardIcon != null)
            {
                cardIcon = cardData.cardIcon;
                if (cardImage != null) cardImage.sprite = cardIcon;
            }

            if (cardData is MonsterCardData monsterData)
            {
                if (attackText != null) attackText.text = monsterData.attack.ToString();
                if (hpText != null) hpText.text = monsterData.hp.ToString();
            }
            else
            {
                if (attackText != null) attackText.text = "";
                if (hpText != null) hpText.text = "";
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
