using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class JapaneseLabelAttribute : PropertyAttribute
{
    public string label;

    public JapaneseLabelAttribute(string label)
    {
        this.label = label;
    }
}