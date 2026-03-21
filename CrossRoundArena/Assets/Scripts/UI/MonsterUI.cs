using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CrossRoundArena.Core;
using CrossRoundArena.Data;

namespace CrossRoundArena.UI
{
    public class MonsterUI : MonoBehaviour
    {
        [JapaneseLabel("基本データ")] public CardData monsterData; // ここを CardData に広げる
        
        [JapaneseLabel("コストテキスト")] public TextMeshProUGUI costText;
        [JapaneseLabel("攻撃力テキスト")] public TextMeshProUGUI attackText;
        [JapaneseLabel("体力テキスト")] public TextMeshProUGUI hpText;
        [JapaneseLabel("モンスター画像")] public Image monsterIcon;

        private MonsterInstance targetMonster;

        public void SetMonster(MonsterInstance monster)
        {
            targetMonster = monster;
            monsterData = monster?.Data;
            UpdateUI();
        }

        public void UpdateUI()
        {
            // インスタンス（実体）優先、なければアタッチされたデータ(SO)
            CardData baseData = (targetMonster != null) ? targetMonster.Data : monsterData;
            if (baseData == null) return;

            // モンスター固有の情報を取得（MonsterCardData型であるか確認）
            MonsterCardData mData = baseData as MonsterCardData;

            // 基本情報の表示 (CardDataにある共通項目)
            if (costText != null) costText.text = baseData.initialCost.ToString();
            if (monsterIcon != null) monsterIcon.sprite = baseData.cardIcon;

            // 攻撃力・体力の表示
            if (targetMonster != null)
            {
                // 実体がある場合は「現在値」を表示
                if (attackText != null) attackText.text = targetMonster.currentAttack.ToString();
                if (hpText != null) hpText.text = targetMonster.currentHP.ToString();
            }
            else if (mData != null)
            {
                // モンスターデータ単体の場合は「初期値」を表示
                if (attackText != null) attackText.text = mData.attack.ToString();
                if (hpText != null) hpText.text = mData.hp.ToString();
            }
        }
    }
}
