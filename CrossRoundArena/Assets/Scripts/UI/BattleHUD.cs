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
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI phaseText; // Current Player's Turn or Event phase
        
        [Header("Timer")]
        public TextMeshProUGUI timerText;
        public Image timerFillImage;

        [Header("Event Display")]
        public GameObject eventPanel;
        public TextMeshProUGUI eventNameText;
        public TextMeshProUGUI eventDescriptionText;

        [Header("Buttons")]
        public Button endTurnButton;

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
