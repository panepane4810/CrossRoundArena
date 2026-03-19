using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CrossRoundArena.Core;

namespace CrossRoundArena.UI
{
    public class PlayerUI : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI coinsText;
        public TextMeshProUGUI costText;
        public Image leaderIcon;
        public Image ghostIcon;

        private PlayerState targetPlayer;

        public void SetPlayer(PlayerState player)
        {
            targetPlayer = player;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (targetPlayer == null) return;

            hpText.text = $"HP: {targetPlayer.currentHP}/{targetPlayer.maxHP}";
            coinsText.text = $"Coins: {targetPlayer.coins}";
            costText.text = $"Cost: {targetPlayer.currentCost}/{targetPlayer.maxCost}";
            
            if (targetPlayer.leader != null)
                leaderIcon.sprite = targetPlayer.leader.leaderIcon;

            if (targetPlayer.status == PlayerStatus.Ghost)
            {
                ghostIcon.gameObject.SetActive(true);
            }
            else
            {
                ghostIcon.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (targetPlayer == null) return;
            TargetSelectionManager.instance.OnTargetClicked(targetPlayer);
        }
    }
}
