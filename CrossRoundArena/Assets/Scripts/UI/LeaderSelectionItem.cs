using UnityEngine;
using UnityEngine.UI;
using CrossRoundArena.Data;
using CrossRoundArena.Core;
using TMPro;

namespace CrossRoundArena.UI
{
    public class LeaderSelectionItem : MonoBehaviour
    {
        [JapaneseLabel("リーダー画像（Image）")] public Image iconImage;
        [JapaneseLabel("リーダー名（Text）")] public TextMeshProUGUI nameText;
        [JapaneseLabel("選択ボタン")] public Button selectButton;

        public void Setup(LeaderData data, System.Action onSelected)
        {
            if (iconImage != null) iconImage.sprite = data.leaderIcon;
            if (nameText != null) nameText.text = data.leaderName;

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => onSelected?.Invoke());
        }
    }
}
