using UnityEngine;
using CrossRoundArena.Core;
using System.Collections.Generic;

namespace CrossRoundArena.Events
{
    [CreateAssetMenu(fileName = "SandstormEvent", menuName = "ScriptableObjects/Events/Sandstorm")]
    public class SandstormEvent : GameEvent
    {
        public int damageAmount = 1;

        public override void Execute(List<PlayerState> players)
        {
            Debug.Log($"Event: {eventName} - All monsters take {damageAmount} damage!");
            foreach (var player in players)
            {
                // Create a copy to avoid modification during iteration
                var monsters = new List<MonsterInstance>(player.boardMonsters);
                foreach (var monster in monsters)
                {
                    monster.TakeDamage(damageAmount, DamageSource.Event);
                }
                // Cleanup dead monsters
                player.boardMonsters.RemoveAll(m => m.IsDead);
            }
        }
    }

    [CreateAssetMenu(fileName = "TyphoonEvent", menuName = "ScriptableObjects/Events/Typhoon")]
    public class TyphoonEvent : GameEvent
    {
        public int drawAmount = 5;

        public override void Execute(List<PlayerState> players)
        {
            Debug.Log($"Event: {eventName} - Hand reset! Draw {drawAmount} cards.");
            foreach (var player in players)
            {
                player.hand.Clear();
                player.DrawCard(drawAmount);
            }
        }
    }

    [CreateAssetMenu(fileName = "MerchantEvent", menuName = "ScriptableObjects/Events/Merchant")]
    public class MerchantEvent : GameEvent
    {
        public override void Execute(List<PlayerState> players)
        {
            Debug.Log($"Event: {eventName} - Merchant appeared! (UI Logic needed)");
            // Here we would typically trigger a UI panel for all players to buy cards
        }
    }
}
