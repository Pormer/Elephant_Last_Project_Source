using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Tutorials
{
    public class TutorialSignal : MonoBehaviour
    {
        public UnityEvent OnEndTutorialEvent;
        public UnityEvent OnStartTutorialEvent;
        
        [SerializeField] private GameEventChannelSO tutorialChannel;
        [field:Space]
        [field:SerializeField] public TutorialType TutorialType { get; private set; }
        [Space]
        [SerializeField] private float endDelay = -1;
        [Space]
        [SerializeField] private bool ignoreStartTutorial;
        
        private bool isFirstStartSignal = true;
        private bool isFirstEndSignal = true;
        
        private void Start()
        {
            Debug.Assert(TutorialType != TutorialType.None,"Tutorial type is undefined or set to None.");
        }

        public void SendEndTutorialSignal()
        {
            if(isFirstEndSignal == false) 
                return;
            
            var evt = TutorialEvents.EndTutorialEvent.Initializer(TutorialType);
            tutorialChannel.RaiseEvent(evt);
            
            isFirstEndSignal = false;
            
            OnEndTutorialEvent?.Invoke();
        }
        
        public void SendStartTutorialSignal()
        {
            if(isFirstStartSignal == false) 
                return;
            
            var evt = TutorialEvents.StartTutorialEvent.Initializer(TutorialType, endDelay);
            tutorialChannel.RaiseEvent(evt);

            isFirstStartSignal = false;
            
            OnStartTutorialEvent?.Invoke();
        }
    }
}