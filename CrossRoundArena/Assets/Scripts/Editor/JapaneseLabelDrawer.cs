#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using CrossRoundArena.Core;

namespace CrossRoundArena.Editor
{
    [CustomPropertyDrawer(typeof(JapaneseLabelAttribute))]
    public class JapaneseLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            JapaneseLabelAttribute attr = (JapaneseLabelAttribute)attribute;
            label.text = attr.label;
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif
