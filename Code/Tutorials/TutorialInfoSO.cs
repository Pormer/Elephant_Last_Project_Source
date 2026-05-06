using UnityEngine;

namespace Code.Tutorials
{
    [CreateAssetMenu(fileName = "TutorialInfo", menuName = "SO/Tutorial/TutorialInfo", order = 0)]
    public class TutorialInfoSO : ScriptableObject
    {
        public TutorialType tutorialType;
        
        [Header("Tutorial UI")]
        [TextArea]
        public string tutorialText;
    }
}