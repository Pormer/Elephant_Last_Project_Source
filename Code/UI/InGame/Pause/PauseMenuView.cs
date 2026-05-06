using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.InGame.Pause
{
    public class PauseMenuView : MonoBehaviour, ISubUIState, ISelectableState
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement element;
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private PauseButton[] pauseButtons;

        public UIStateType StateType => UIStateType.Pause;
        public GameObject GameObject => gameObject;
        public Selectable DefaultFocusable => pauseButtons?[0].PointerHandler;
        private const string HideKey = "hide";

        private void Awake()
        {
            foreach (var pb in pauseButtons)
                pb.OnPressed += HandlePressed;
        }

        private void OnDestroy()
        {
            foreach (var pb in pauseButtons)
                pb.OnPressed -= HandlePressed;
        }

        private void HandlePressed(UIStateType obj)
        {
            switch (obj)
            {
                case UIStateType.Setting:
                    uiChannel.RaiseEvent(UIEvents.SubUIChangeEvent.Initializer(UIStateType.Setting));
                    break;
                case UIStateType.MainHud:
                    uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
                    break;
                case UIStateType.Title:
                    GoToTitle();
                    break;
            }
        }

        private void GoToTitle()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(true, Application.Quit));
        }

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await element.RemoveState(HideKey);
        }

        public async UniTask OnExit()
        {
            await element.AddState(HideKey);
            canvas.enabled = false;
        }
    }
}