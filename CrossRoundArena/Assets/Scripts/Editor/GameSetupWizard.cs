#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using CrossRoundArena.Core;
using CrossRoundArena.UI;
using UnityEngine.EventSystems;

namespace CrossRoundArena.Editor
{
    public class GameSetupWizard : EditorWindow
    {
        [MenuItem("Tools/CrossRoundArena/Setup Game Scene")]
        public static void SetupScene()
        {
            // 1. Core Objects
            CreateCoreObject<GameManager>("GameManager");
            CreateCoreObject<TargetSelectionManager>("SelectionManager");

            // 2. UI - EventSystem
            if (FindAnyObjectByType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            // 3. UI - Canvas & BattleHUD
            GameObject canvasObj = CreateCanvasWithHUD();
            BattleHUD hud = canvasObj.GetComponent<BattleHUD>();

            // 4. Player UI Area
            GameObject playerStatsArea = new GameObject("PlayerStatsArea", typeof(RectTransform));
            playerStatsArea.transform.SetParent(canvasObj.transform, false);
            SetRect(playerStatsArea, new Vector2(0, 1), new Vector2(0, 1), new Vector2(200, -200), new Vector2(400, 400));
            
            // Add vertical layout for players
            var layout = playerStatsArea.AddComponent<VerticalLayoutGroup>();
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            layout.spacing = 10;

            for (int i = 1; i <= 4; i++)
            {
                CreatePlayerUI(playerStatsArea.transform, $"PlayerUI_P{i}");
            }

            // 5. Monster Area (Field)
            GameObject monsterArea = new GameObject("MonsterBoard", typeof(RectTransform));
            monsterArea.transform.SetParent(canvasObj.transform, false);
            SetRect(monsterArea, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(800, 200));
            var boardLayout = monsterArea.AddComponent<HorizontalLayoutGroup>();
            boardLayout.spacing = 20;
            boardLayout.childAlignment = TextAnchor.MiddleCenter;

            for (int i = 1; i <= 5; i++)
            {
                CreateMonsterUI(monsterArea.transform, $"MonsterSlot_{i}");
            }

            // 6. Hand Area
            GameObject handArea = new GameObject("HandArea", typeof(RectTransform));
            handArea.transform.SetParent(canvasObj.transform, false);
            SetRect(handArea, new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 100), new Vector2(1000, 150));
            var handLayout = handArea.AddComponent<HorizontalLayoutGroup>();
            handLayout.spacing = -20; // Fan style overlap
            handLayout.childAlignment = TextAnchor.LowerCenter;

            Selection.activeGameObject = canvasObj;
            Debug.Log("Cross Round Arena: Scene Setup Complete!");
        }

        private static void CreateCoreObject<T>(string name) where T : MonoBehaviour
        {
            if (FindAnyObjectByType<T>() == null)
            {
                GameObject go = new GameObject(name);
                go.AddComponent<T>();
            }
        }

        private static GameObject CreateCanvasWithHUD()
        {
            GameObject canvasObj = new GameObject("Canvas_Battle", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            BattleHUD hud = canvasObj.AddComponent<BattleHUD>();

            // Round Text
            hud.roundText = CreateText(canvasObj.transform, "RoundText", "ROUND 1", new Vector2(0, 50), new Vector2(200, 50), Color.yellow);
            // Timer Text
            hud.timerText = CreateText(canvasObj.transform, "TimerText", "30", new Vector2(0, 0), new Vector2(100, 100), Color.white, 40);
            
            // End Turn Button
            GameObject btnObj = new GameObject("EndTurnButton", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(canvasObj.transform, false);
            SetRect(btnObj, new Vector2(1, 0), new Vector2(1, 0), new Vector2(-100, 50), new Vector2(150, 60));
            hud.endTurnButton = btnObj.GetComponent<Button>();
            CreateText(btnObj.transform, "Text", "END TURN", Vector2.zero, new Vector2(150, 60), Color.black, 20);

            // Event Panel
            GameObject panel = new GameObject("EventPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panel.transform.SetParent(canvasObj.transform, false);
            SetRect(panel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(600, 300));
            panel.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
            hud.eventPanel = panel;
            
            hud.eventNameText = CreateText(panel.transform, "EventName", "EVENT", new Vector2(0, 100), new Vector2(500, 50), Color.red, 30);
            hud.eventDescriptionText = CreateText(panel.transform, "EventDesc", "Something happens...", Vector2.zero, new Vector2(500, 150), Color.white, 20);
            panel.SetActive(false);

            return canvasObj;
        }

        private static void CreatePlayerUI(Transform parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 80);
            go.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            
            PlayerUI ui = go.AddComponent<PlayerUI>();
            ui.hpText = CreateText(go.transform, "HP", "HP: 30/30", new Vector2(-50, 0), new Vector2(150, 30), Color.green);
            ui.coinsText = CreateText(go.transform, "Coins", "Coins: 0", new Vector2(50, 20), new Vector2(150, 30), Color.yellow);
            ui.costText = CreateText(go.transform, "Cost", "Cost: 1/1", new Vector2(50, -20), new Vector2(150, 30), Color.cyan);
            
            GameObject icon = new GameObject("LeaderIcon", typeof(RectTransform), typeof(Image));
            icon.transform.SetParent(go.transform, false);
            SetRect(icon, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(40, 0), new Vector2(60, 60));
            ui.leaderIcon = icon.GetComponent<Image>();

            GameObject ghost = new GameObject("GhostIcon", typeof(RectTransform), typeof(Image));
            ghost.transform.SetParent(go.transform, false);
            SetRect(ghost, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -20), new Vector2(40, 40));
            ui.ghostIcon = ghost.GetComponent<Image>();
            ghost.SetActive(false);
        }

        private static void CreateMonsterUI(Transform parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 180);
            go.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            MonsterUI ui = go.AddComponent<MonsterUI>();
            ui.attackText = CreateText(go.transform, "ATK", "0", new Vector2(-40, -70), new Vector2(40, 30), Color.orange, 24);
            ui.hpText = CreateText(go.transform, "HP", "0", new Vector2(40, -70), new Vector2(40, 30), Color.red, 24);
            
            GameObject icon = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            icon.transform.SetParent(go.transform, false);
            SetRect(icon, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 20), new Vector2(100, 100));
            ui.monsterIcon = icon.GetComponent<Image>();
        }

        private static TextMeshProUGUI CreateText(Transform parent, string name, string content, Vector2 pos, Vector2 size, Color color, int fontSize = 18)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = TextAlignmentOptions.Center;

            return text;
        }

        private static void SetRect(GameObject go, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size)
        {
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
        }
    }
}
#endif
