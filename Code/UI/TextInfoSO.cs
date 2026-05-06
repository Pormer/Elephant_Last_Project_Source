using UnityEngine;

namespace Code.UI
{
    [CreateAssetMenu(fileName = "Text info", menuName = "SO/UI/Text info", order = 0)]
    public class TextInfoSO : ScriptableObject
    {
        public string textName;
        [ColorUsage(true, true)]
        public Color textColor;

        public int nameHash;
        public float fontSize;

        private void OnValidate()
        {
            nameHash = Animator.StringToHash(textName);
        }
    }
}