using TMPro;
using UnityEngine;

namespace Code.Tutorials
{
    public class TutorialPanel : MonoBehaviour
    {
        [field:SerializeField] public TutorialType TutorialType { get; private set; }
        [SerializeField] protected TextMeshProUGUI textMeshProUGUI;
        
        private TutorialCanvas _tutorialCanvas;
        
        private void Start()
        {
            if(TutorialType == TutorialType.None)
                Debug.LogError("Tutorial type is undefined or set to None.");
        }

        public void Initialize(TutorialCanvas tutorialCanvas)
        {
            _tutorialCanvas = tutorialCanvas;
            HideTutorialPanel();
        }
        
        public void SetUpTutorialPanel(TutorialInfoSO tutorialInfo)
        {
            textMeshProUGUI.text = tutorialInfo.tutorialText;
        }

        public void ShowTutorialPanel()
        {
            gameObject.SetActive(true);
        }
        
        public void HideTutorialPanel()
        {
            gameObject.SetActive(false);
        }
    }
}