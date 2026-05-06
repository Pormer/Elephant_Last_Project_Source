using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Pause
{
    public class PauseView : SubUIStateMachine, ISelectableState
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement element;
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private GameEventChannelSO uiChannel;

        #region UI State

        public override UIStateType StateType => UIStateType.Pause;
        public override bool DoesStop => true;

        #endregion

        private const string HideKey = "hide";

        public override async UniTask OnEnter()
        {
            canvas.enabled = true;
            await base.OnEnter();
            await element.RemoveState(HideKey);
            uiInput.OnCancelPressed += HandleCancel;
            uiInput.OnPausePressed += HandlePause;
            uiChannel.AddListener<SubUICancelEvent>(HandleSubUICancel);
        }

        public override async UniTask OnExit()
        {
            uiInput.OnCancelPressed -= HandleCancel;
            uiInput.OnPausePressed -= HandlePause;
            await base.OnExit();
            await element.AddState(HideKey);
            canvas.enabled = false;
            uiChannel.RemoveListener<SubUICancelEvent>(HandleSubUICancel);
        }

        private void HandleSubUICancel(SubUICancelEvent obj)
        {
            uiEventChannel.RaiseEvent(UIEvents.SubUIChangeEvent.Initializer(UIStateType.Pause));
        }

        private void HandleCancel()
        {
            if (CurrentStateType == UIStateType.Pause)
                uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
        }

        private void HandlePause()
        {
            if (CurrentStateType == UIStateType.Pause)
                uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
        }

        public Selectable DefaultFocusable => CurrentState is not ISelectableState focusableState
            ? null
            : focusableState.DefaultFocusable;
    }
}