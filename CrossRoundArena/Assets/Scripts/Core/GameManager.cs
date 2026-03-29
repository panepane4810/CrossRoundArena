using CrossRoundArena.Events;
using CrossRoundArena.UI;
using UnityEngine;
using System.Collections.Generic;
using CrossRoundArena.Data;

namespace CrossRoundArena.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public List<PlayerState> activePlayers = new List<PlayerState>();
        public List<GameEvent> eventPool = new List<GameEvent>();
        [JapaneseLabel("現在のイベント")] public GameEvent currentActiveEvent;
        [JapaneseLabel("現在のラウンド")] public int currentRound = 1;
        [JapaneseLabel("手番プレイヤー")] public int currentPlayerIndex = 0;
        [JapaneseLabel("制限時間")] public float turnTimeLimit = 30f;
        [JapaneseLabel("残り時間")] public float currentTurnTimeRemaining;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void BeginSelection()
        {
            if (LeaderSelectionUI.instance != null)
            {
                LeaderSelectionUI.instance.StartSelection();
            }
            else
            {
                StartGame();
            }
        }

        public void StartGame()
        {
            InitializePlayers();
            
            // 全PlayerUIを更新して、選択したリーダーを表示
            var playerUIs = UnityEngine.Object.FindObjectsByType<UI.PlayerUI>(FindObjectsSortMode.None);
            foreach (var ui in playerUIs)
            {
                ui.UpdateUI();
            }

            StartTurn();
        }

        private void InitializePlayers()
        {
            foreach (var player in activePlayers)
            {
                if (player.leader != null)
                {
                    player.currentHP = player.leader.initialHP;
                    player.maxHP = player.leader.initialHP;
                }
                player.currentCost = 0;
                player.maxCost = 1;
                player.coins = 0;
            }
        }

        public void StartTurn()
        {
            currentTurnTimeRemaining = turnTimeLimit;
            var player = activePlayers[currentPlayerIndex];

            // ターンの初期化処理
            player.maxCost = Mathf.Min(player.maxCost + 1, 10);
            player.currentCost = player.maxCost;

            // コインの獲得（毎ターンの獲得額は要件に合わせて調整）
            player.coins += 2; 

            // カードのドロー
            player.DrawCard(1);

            // モンスターの攻撃フラグリセット
            foreach (var monster in player.boardMonsters)
            {
                monster.RefreshTurn();
            }

            // 幽霊状態のカウントダウン
            player.HandleGhostTurn();

            // UIの更新
            if (BattleHUD.instance != null)
            {
                BattleHUD.instance.UpdateRoundInfo(currentRound, player.playerName);
            }
            
            foreach (var pUI in FindObjectsByType<PlayerUI>(FindObjectsSortMode.None))
            {
                pUI.UpdateUI();
            }
        }

        public void EndTurn()
        {
            // 次のプレイヤーへ
            currentPlayerIndex++;
            if (currentPlayerIndex >= activePlayers.Count)
            {
                currentPlayerIndex = 0;
                EndRound();
            }
            else
            {
                StartTurn();
            }
        }

        public void EndRound()
        {
            // ラウンド終了時のイベント発生
            currentRound++;
            TriggerEvent();
        }

        private void TriggerEvent()
        {
            if (eventPool.Count == 0)
            {
                Debug.LogWarning("Event pool is empty!");
                StartTurn();
                return;
            }

            // Randomly select event
            currentActiveEvent = eventPool[UnityEngine.Random.Range(0, eventPool.Count)];
            Debug.Log($"Round {currentRound-1} End Event: {currentActiveEvent.eventName}");

            // Execute the event
            currentActiveEvent.Execute(activePlayers);

            // Notify UI
            if (BattleHUD.instance != null)
            {
                BattleHUD.instance.ShowEvent(currentActiveEvent.eventName, currentActiveEvent.description);
            }

            // Show event for 3 seconds, then start next turn
            CancelInvoke("StartTurn");
            Invoke("StartTurn", 3.0f);
        }

        private void Update()
        {
            if (currentTurnTimeRemaining > 0)
            {
                currentTurnTimeRemaining -= Time.deltaTime;
                
                if (BattleHUD.instance != null)
                {
                    BattleHUD.instance.UpdateTimer(currentTurnTimeRemaining, turnTimeLimit);
                }

                if (currentTurnTimeRemaining <= 0)
                {
                    EndTurn();
                }
            }
        }
    }
}
