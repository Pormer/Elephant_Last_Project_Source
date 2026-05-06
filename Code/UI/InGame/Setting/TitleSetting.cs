using Code.UI.UIStateMachine;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.InGame.Setting
{
    public class TitleSetting : SettingView, IMainUIState
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        public UIStateType StateType => UIStateType.Setting;
        public GameObject GameObject => gameObject;
        public bool DoesStop => true;

        private const string HideKey = "hide";

        private bool _subElementFocused;

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            OnRevertScreen?.Invoke();
            await element.RemoveState(HideKey);
            IsCanceling = false;
            uiInput.OnCancelPressed += HandleCancel;
        }

        public async UniTask OnExit()
        {
            uiInput.OnCancelPressed -= HandleCancel;
            await element.AddState(HideKey);
            canvas.enabled = false;
        }

        private void HandleCancel()
        {
            if (!IsKeyRebinding && !_subElementFocused)
            {
                IsCanceling = true;
                uiChannel.RaiseEvent(UIEvents.UIFadeInOutEvent.Initializer(() =>
                    uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Title))));
            }
        }

        protected override void HandleSubElementFocused(bool obj)
        {
            base.HandleSubElementFocused(obj);
            _subElementFocused = obj;
        }
    }
}