using UnityEngine;
using CrossRoundArena.Core;

namespace CrossRoundArena.Data
{
    [CreateAssetMenu(fileName = "LeaderData", menuName = "ScriptableObjects/LeaderData", order = 1)]
    public class LeaderData : ScriptableObject
    {
        [JapaneseLabel("リーダータイプ")] public LeaderType leaderType;
        [JapaneseLabel("リーダー名")] public string leaderName;
        [JapaneseLabel("初期HP")] public int initialHP;
        [JapaneseLabel("特徴説明")] public string traitDescription;
        [JapaneseLabel("アイコン")] public Sprite leaderIcon;
        [JapaneseLabel("死亡アイコン")] public Sprite ghostIcon;

        // リーダー固有の効果や能力をここに定義
    }
}
