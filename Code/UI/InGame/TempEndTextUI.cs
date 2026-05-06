using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;

namespace Code.UI.InGame
{
    public class TempEndTextUI : MonoBehaviour, IMainUIState
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement element;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private UIInputSO inputSO;
        public UIStateType StateType => UIStateType.TempEnd;
        public GameObject GameObject => gameObject;

        private const string ShowKey = "show";

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await element.AddState(ShowKey);
            inputSO.OnPausePressed += HandlePause;
        }

        public async UniTask OnExit()
        {
            inputSO.OnPausePressed -= HandlePause;
            await element.RemoveState(ShowKey);
            canvas.enabled = false;
        }

        private void HandlePause()
        {
            Application.Quit();
        }

        public bool DoesStop => true;

        public void EndBeta()
        {
            uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.TempEnd));
        }
    }
}