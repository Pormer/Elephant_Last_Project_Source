using Code.UI.UIStateMachine;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.InGame.Setting
{
    public class InGameSetting : SettingView, ISubUIState
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        public UIStateType StateType => UIStateType.Setting;
        public GameObject GameObject => gameObject;

        private bool _isSubFocused;

        private const string HideKey = "hide";

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            OnRevertScreen?.Invoke();
            await element.RemoveState(HideKey);
            IsCanceling = false;
            uiInput.OnCancelPressed += HandleCancel;
            uiInput.OnPausePressed += HandleCancel;
        }

        public async UniTask OnExit()
        {
            uiInput.OnCancelPressed -= HandleCancel;
            uiInput.OnPausePressed -= HandleCancel;
            await element.AddState(HideKey);
            canvas.enabled = false;
        }

        private void HandleCancel()
        {
            Debug.Log($"Is key rebinding: {IsKeyRebinding}");
            if (!_isSubFocused && !IsKeyRebinding)
            {
                IsCanceling = true;
                uiChannel.RaiseEvent(UIEvents.SubUICancelEvent);
            }
        }

        protected override void HandleSubElementFocused(bool obj)
        {
            base.HandleSubElementFocused(obj);
            _isSubFocused = obj;
        }
    }
}
