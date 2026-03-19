using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CrossRoundArena.Core;

namespace CrossRoundArena.UI
{
    public class MonsterUI : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI hpText;
        public Image monsterIcon;
        public Image guardIcon; // ガード持ちの場合のアイコン

        private MonsterInstance monsterInstance;

        public void SetMonster(MonsterInstance monster)
        {
            monsterInstance = monster;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (monsterInstance == null) return;

            attackText.text = monsterInstance.currentAttack.ToString();
            hpText.text = $"{monsterInstance.currentHP}/{monsterInstance.maxHP}";
            
            if (guardIcon != null)
                guardIcon.gameObject.SetActive(monsterInstance.HasKeyword(Keyword.Guard));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (monsterInstance == null) return;

            // TargetSelectionManagerに通知
            if (TargetSelectionManager.instance.currentMode != SelectionMode.None)
            {
                TargetSelectionManager.instance.OnTargetClicked(monsterInstance);
            }
            else
            {
                // セレクションモードでない場合は、攻撃開始の起点になる
                // (例: 自分のモンスターをクリックして敵を狙う攻撃モードへ)
                if (monsterInstance.owner == GameManager.instance.activePlayers[GameManager.instance.currentPlayerIndex])
                {
                    TargetSelectionManager.instance.StartAttackTargetSelection(monsterInstance, target => 
                    {
                        CombatSystem.ExecuteAttack(monsterInstance, target);
                        UpdateUI(); // 攻撃後のステータス反映
                    });
                }
            }
        }
    }
}
