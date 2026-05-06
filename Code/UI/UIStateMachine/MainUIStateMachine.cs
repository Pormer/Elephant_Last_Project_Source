using EventSystem;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public class MainUIStateMachine : UIStateMachine<IMainUIState, UIChangeEvent>
    {
        protected override void Start()
        {
            base.Start();
            InitializeStates();
            Time.timeScale = CurrentState.DoesStop ? 0f : 1f;
        }

        protected override void HandleExitState(IMainUIState prevState)
        {
            base.HandleExitState(prevState);
            uiEventChannel.RaiseEvent(UIEvents.InputChangeEvent.Initializer(false, false));
        }

        protected override void HandleNewState(IMainUIState newState)
        {
            base.HandleNewState(newState);
            
            Time.timeScale = newState.DoesStop ? 0f : 1f;
            uiEventChannel.RaiseEvent(UIEvents.InputChangeEvent.Initializer(!newState.DoesStop, true));
        }
    }
}