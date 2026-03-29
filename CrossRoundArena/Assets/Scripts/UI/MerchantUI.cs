using UnityEngine;
using CrossRoundArena.Core;
using CrossRoundArena.Data;
using System.Collections.Generic;

namespace CrossRoundArena.UI
{
    public class MerchantUI : MonoBehaviour
    {
        public static MerchantUI instance;

        [JapaneseLabel("ショップパネル")] public GameObject panel;
        [JapaneseLabel("カードコンテナ")] public Transform cardContainer;
        [JapaneseLabel("カードプレハブ")] public GameObject cardPrefab; // CardUI component attached

        private List<CardData> availableCards = new List<CardData>();
        private PlayerState shoppingPlayer;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
            
            if (panel != null) panel.SetActive(false);
        }

        public void Open(PlayerState player, List<CardData> offerCards)
        {
            shoppingPlayer = player;
            availableCards = offerCards;
            panel.SetActive(true);

            // Clear old icons
            foreach (Transform child in cardContainer) Destroy(child.gameObject);

            // Create card buttons
            foreach (var card in offerCards)
            {
                var cardObj = Instantiate(cardPrefab, cardContainer);
                var cardUI = cardObj.GetComponent<CardUI>();
                cardUI.SetCard(card);
                
                // Override click behavior for merchant
                var btn = cardObj.GetComponent<UnityEngine.UI.Button>();
                if (btn == null) btn = cardObj.AddComponent<UnityEngine.UI.Button>();
                
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => BuyCard(card));
            }
        }

        private void BuyCard(CardData card)
        {
            if (shoppingPlayer.coins >= card.purchaseCoin)
            {
                shoppingPlayer.coins -= card.purchaseCoin;
                shoppingPlayer.deck.Add(card);
                Debug.Log($"{shoppingPlayer.playerName} bought {card.cardName}!");
                Close();
            }
            else
            {
                Debug.LogWarning("Not enough coins!");
            }
        }

        public void Close()
        {
            panel.SetActive(false);
        }
    }
}
