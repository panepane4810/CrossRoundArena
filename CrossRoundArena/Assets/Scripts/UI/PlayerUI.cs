using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CrossRoundArena.Core;

namespace CrossRoundArena.UI
{
    public class PlayerUI : MonoBehaviour, IPointerClickHandler
    {
        [JapaneseLabel("HPテキスト")] public TextMeshProUGUI hpText;
        [JapaneseLabel("コインテキスト")] public TextMeshProUGUI coinsText;
        [JapaneseLabel("コストテキスト")] public TextMeshProUGUI costText;
        [JapaneseLabel("リーダーアイコン")] public Image leaderIcon;
        [JapaneseLabel("死亡アイコン")] public Image ghostIcon;

         public PlayerState targetPlayer;

        public void SetPlayer(PlayerState player)
        {
            targetPlayer = player;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (targetPlayer == null) return;

            // 本体（PlayerUI自身）のImageをリーダーアイコンにする
            Image mainImage = GetComponent<Image>();
            if (mainImage != null && targetPlayer.leader != null)
            {
                mainImage.sprite = targetPlayer.leader.leaderIcon;
            }

            // 各UIコンポーネントのアサインを確認しながら安全に更新
            if (hpText != null) hpText.text = $"HP: {targetPlayer.currentHP}/{targetPlayer.maxHP}";
            if (coinsText != null) coinsText.text = $"Coins: {targetPlayer.coins}";
            if (costText != null) costText.text = $"Cost: {targetPlayer.currentCost}/{targetPlayer.maxCost}";
            
            if (targetPlayer.leader != null)
            {
                if (leaderIcon != null) leaderIcon.sprite = targetPlayer.leader.leaderIcon;
                if (ghostIcon != null) ghostIcon.sprite = targetPlayer.leader.ghostIcon;
            }

            if (ghostIcon != null)
            {
                ghostIcon.gameObject.SetActive(targetPlayer.status == PlayerStatus.Ghost);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (targetPlayer == null) return;
            TargetSelectionManager.instance.OnTargetClicked(targetPlayer);
        }
    }
}
