using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Tutorials
{
    [Provide, DefaultExecutionOrder(-10)]
    public class TutorialManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private GameEventChannelSO tutorialChannel;
        [SerializeField] private TutorialInfoSO[] tutorialInfoData;

        private Dictionary<TutorialType, TutorialInfoSO> _tutorialInfoDict;
        
        private TutorialInfoSO _currentTutorialInfo;
        
        [Inject] private TutorialCanvas _tutorialCanvas;

        private void Awake()  
        {
            _tutorialInfoDict = tutorialInfoData.ToDictionary(data => data.tutorialType);
            
            tutorialChannel.AddListener<StartTutorialEvent>(HandleStartTutorialEvent);
            tutorialChannel.AddListener<EndTutorialEvent>(HandleEndTutorialEvent);
        }

        private void OnDestroy()
        {
            tutorialChannel.RemoveListener<StartTutorialEvent>(HandleStartTutorialEvent);
            tutorialChannel.RemoveListener<EndTutorialEvent>(HandleEndTutorialEvent);
        }

        private void HandleEndTutorialEvent(EndTutorialEvent evt)
        {
            Debug.Log($"Tutorial {evt.tutorialType} started");
            
            if (_currentTutorialInfo == null)
            {
                Debug.LogError($"시작한 {_currentTutorialInfo} 튜토리얼이 없습니다. 시작 트리거에 닿지 않고 바로 끝 트리거에 닿았을 확률이 높습니다.");
                return;
            }
            
            if(_currentTutorialInfo.tutorialType != evt.tutorialType) 
            {
                Debug.LogError($"현재 진행 중인 {_currentTutorialInfo.tutorialType} 튜토리얼과 EndSignal의 튜토리얼이 다릅니다. 시작 트리거에 닿지 않고 바로 끝 트리거에 닿았을 확률이 높습니다.");
                return;
            }
            
            _tutorialCanvas.HidePanel();
            _currentTutorialInfo = null;
        }

        private void HandleStartTutorialEvent(StartTutorialEvent evt)
        {
            Debug.Log($"Tutorial {evt.tutorialType} end");
            
            if (_currentTutorialInfo != null)
            {
                Debug.LogError($"아직 {_currentTutorialInfo} 튜토리얼을 진행 중입니다.");
                return;
            }

            if (_tutorialInfoDict.TryGetValue(evt.tutorialType, out var tutorialInfo) == false)
            {
                Debug.LogError($"{evt.tutorialType}와 맞는 TutorialInfoSO가 없습니다. 리스트에 SO 추가 좀");
                return;
            }
            
            _tutorialCanvas.ShowPanel(tutorialInfo);
            _currentTutorialInfo = tutorialInfo;

            if (evt.endDelay >= 0)
            {
                StartCoroutine(EndDelay(evt.endDelay));
            }
        }

        private IEnumerator EndDelay(float endDelay)
        {
            yield return new WaitForSeconds(endDelay);
            
            _tutorialCanvas.HidePanel();
            _currentTutorialInfo = null;
        }
    }
}
