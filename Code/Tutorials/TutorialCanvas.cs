using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Tutorials
{
    [Provide]
    public class TutorialCanvas : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration;
        
        private Dictionary<TutorialType, TutorialPanel> tutorialPanelDict;
        private TutorialPanel _currentTutorialPanel;
        private bool _isFade;
        
        private void Awake()
        {
            tutorialPanelDict = GetComponentsInChildren<TutorialPanel>()
                .ToDictionary(panel => panel.TutorialType);
            
            foreach (var tutorialPanel in tutorialPanelDict)
                tutorialPanel.Value.Initialize(this);
            
            canvasGroup.alpha = 0f;
        }
        
        public void ShowPanel(TutorialInfoSO tutorialInfo)
        {
            if (tutorialPanelDict.TryGetValue(tutorialInfo.tutorialType, out var tutorialPanel) == false)
            {
                Debug.Assert(tutorialPanel != null,$"Tutorial Panel {tutorialInfo.tutorialType} is not found");
                return;
            }
            
            if (_isFade)
            {
                _currentTutorialPanel.HideTutorialPanel();
                _currentTutorialPanel = null;
                _isFade = false;
            }
            
            _currentTutorialPanel = tutorialPanel;
            _currentTutorialPanel.SetUpTutorialPanel(tutorialInfo);
            
            //TutorialPanel은 무조건 하나만 보여줄 거기 때문에 CanvasGroup 하나로 조정
            _currentTutorialPanel.ShowTutorialPanel();
            
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration);
        }

        public void HidePanel()
        {
            _isFade = true;
            
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                _currentTutorialPanel.HideTutorialPanel();
                _currentTutorialPanel = null;
                _isFade = false;
            });
        }
    }
}