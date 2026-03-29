using UnityEngine;
using CrossRoundArena.Data;
using CrossRoundArena.Core;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace CrossRoundArena.UI
{
    public class LeaderSelectionUI : MonoBehaviour
    {
        public static LeaderSelectionUI instance;

        [JapaneseLabel("選択画面パネル")] public GameObject panel;
        
        [Header("Data & UI Links")]
        [JapaneseLabel("リーダープール")] public List<LeaderData> leaderPool = new List<LeaderData>();
        [JapaneseLabel("反映先のPlayerUIリスト")] public List<PlayerUI> playerUIs = new List<PlayerUI>();
        
        [JapaneseLabel("状況説明ヘッダー")] public TextMeshProUGUI playerNameHeader;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        // ゲーム開始時にボタン群を表示する
        public void StartSelection()
        {
            panel.SetActive(true);
            if (playerNameHeader != null) playerNameHeader.text = "リーダーを選択してください";
        }

        /// <summary>
        /// ヒエラルキー上の各ボタンのOnClickに、インデックス番号を指定してセットしてください
        /// 例: 1つ目のボタン -> OnLeaderBtnClicked(0)
        /// </summary>
        public void OnLeaderBtnClicked(int index)
        {
            if (index < 0 || index >= leaderPool.Count)
            {
                Debug.LogError($"Leader index {index} is out of range!");
                return;
            }

            LeaderData pickedLeader = leaderPool[index];
            
            // プレイヤー1に代入
            if (GameManager.instance.activePlayers.Count > 0)
            {
                var player = GameManager.instance.activePlayers[0];
                player.leader = pickedLeader;
                player.currentHP = pickedLeader.initialHP;
                player.maxHP = pickedLeader.initialHP;

                Debug.Log($"Player 1 selected {pickedLeader.leaderName}");

                // ここで PlayerUI と PlayerState を確実に紐づける
                if (playerUIs.Count > 0 && playerUIs[0] != null)
                {
                    playerUIs[0].targetPlayer = player; // 紐づけ
                    playerUIs[0].UpdateUI(); // 更新
                }
            }

            panel.SetActive(false);
            GameManager.instance.StartGame();
        }
    }
}
