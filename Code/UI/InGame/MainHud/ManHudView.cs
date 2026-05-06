using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;

namespace Code.UI.InGame.MainHud
{
    public class ManHudView : ViewBase, IMainUIState
    {
        #region UI State

        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement canvasElement;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private UIInputSO uiInput;
        public UIStateType StateType => UIStateType.MainHud;
        public GameObject GameObject => gameObject;
        public bool DoesStop => false;

        private const string HideKey = "hide";

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await canvasElement.RemoveState(HideKey);
            uiInput.OnMenuPressed += HandleTabPressed;
            uiInput.OnPausePressed += HandlePausePressed;
        }

        public async UniTask OnExit()
        {
            uiInput.OnMenuPressed -= HandleTabPressed;
            uiInput.OnPausePressed -= HandlePausePressed;
            await canvasElement.AddState(HideKey);
            canvas.enabled = false;
        }


        private void HandleTabPressed() =>
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Menu));
        private void HandlePausePressed()=>
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Pause));

        #endregion
    }
}