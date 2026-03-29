using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CrossRoundArena.Core;

namespace CrossRoundArena.UI
{
    public class BattleHUD : MonoBehaviour
    {
        public static BattleHUD instance;

        [Header("Round Info")]
        [JapaneseLabel("ラウンドテキスト")] public TextMeshProUGUI roundText;
        [JapaneseLabel("フェーズテキスト")] public TextMeshProUGUI phaseText; // Current Player's Turn or Event phase
        
        [Header("Timer")]
        [JapaneseLabel("タイマーテキスト")] public TextMeshProUGUI timerText;
        [JapaneseLabel("タイマーゲージ")] public Image timerFillImage;

        [Header("Event Display")]
        [JapaneseLabel("イベントパネル")] public GameObject eventPanel;
        [JapaneseLabel("イベント名テキスト")] public TextMeshProUGUI eventNameText;
        [JapaneseLabel("イベント詳細テキスト")] public TextMeshProUGUI eventDescriptionText;

        [Header("Buttons")]
        [JapaneseLabel("ターン終了ボタン")] public Button endTurnButton;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);

            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(OnEndTurnClicked);
        }

        public void UpdateRoundInfo(int round, string playerName)
        {
            roundText.text = $"ROUND {round}";
            phaseText.text = $"{playerName}'s Turn";
            HideEventPanel();
        }

        public void UpdateTimer(float remaining, float limit)
        {
            timerText.text = Mathf.CeilToInt(remaining).ToString();
            if (timerFillImage != null)
                timerFillImage.fillAmount = remaining / limit;

            // Change color to red when low
            if (remaining < 5f)
                timerText.color = Color.red;
            else
                timerText.color = Color.white;
        }

        public void ShowEvent(string eventName, string description)
        {
            eventPanel.SetActive(true);
            eventNameText.text = eventName;
            eventDescriptionText.text = description;
            phaseText.text = "EVENT PHASE";
        }

        public void HideEventPanel()
        {
            eventPanel.SetActive(false);
        }

        private void OnEndTurnClicked()
        {
            GameManager.instance.EndTurn();
        }
    }
}
