using UnityEngine;
using CrossRoundArena.Core;
using System.Collections.Generic;

namespace CrossRoundArena.Events
{
    public abstract class GameEvent : ScriptableObject
    {
        public string eventName;
        [TextArea] public string description;
        public Core.EventType eventType;

        public abstract void Execute(List<PlayerState> players);
    }
}
