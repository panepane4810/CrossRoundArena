using UnityEngine;
using CrossRoundArena.Core;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "LeaderData", menuName = "ScriptableObjects/LeaderData", order = 1)]
    public class LeaderData : ScriptableObject
    {
        public LeaderType leaderType;
        public string leaderName;
        public int initialHP;
        public string traitDescription;
        public Sprite leaderIcon;

        // リーダー固有の効果や能力をここに定義
    }
}
