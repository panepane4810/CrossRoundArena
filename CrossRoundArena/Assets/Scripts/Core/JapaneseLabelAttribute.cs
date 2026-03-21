using UnityEngine;

namespace CrossRoundArena.Core
{
    public class JapaneseLabelAttribute : PropertyAttribute
    {
        public string label;

        public JapaneseLabelAttribute(string label)
        {
            this.label = label;
        }
    }
}
