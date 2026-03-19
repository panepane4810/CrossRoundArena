using UnityEngine;
using CrossRoundArena.Core;
using CrossRoundArena.Data;
using System.Collections.Generic;

namespace CrossRoundArena.Events
{
    [CreateAssetMenu(fileName = "BossEvent", menuName = "ScriptableObjects/Events/Boss")]
    public class BossEvent : GameEvent
    {
        public MonsterCardData bossData;
        public int coinRewardPerDamage = 1;
        public int killRewardBase = 5;

        public override void Execute(List<PlayerState> players)
        {
            Debug.Log($"Event: Boss {bossData.cardName} Alert!");
            // Implementation note: 
            // 実際にはボスを特定の共通盤面に召喚し、各プレイヤーが攻撃できるようにするフローが必要
            // 企画書では「ダメージ量に応じて報酬」「横取りが発生する」
        }
    }
}
