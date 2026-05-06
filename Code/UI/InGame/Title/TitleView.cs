using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.InGame.Title
{
    public class TitleView : MonoBehaviour, IMainUIState, ISelectableState
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement element;
        [SerializeField] private int startSceneIndex;
        [Header("Buttons")] [SerializeField] private PointerHandler startButton;
        [SerializeField] private PointerHandler settingButton;
        [SerializeField] private PointerHandler quitButton;
        public Selectable DefaultFocusable => startButton;

        public UIStateType StateType => UIStateType.Title;
        public GameObject GameObject => gameObject;
        public bool DoesStop => false;
        private const string HideKey = "hide";

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await element.RemoveState(HideKey);
            startButton.Pressed += HandleStart;
            settingButton.Pressed += HandleSetting;
            quitButton.Pressed += HandleQuit;
        }

        public async UniTask OnExit()
        {
            startButton.Pressed -= HandleStart;
            settingButton.Pressed -= HandleSetting;
            quitButton.Pressed -= HandleQuit;
            await element.AddState(HideKey);
            canvas.enabled = false;
        }

        private void HandleStart()
        {
            uiChannel.RaiseEvent(
                UIEvents.UIFadeStartEvent.Initializer(true, ChangeScene));
        }

        private async void ChangeScene()
        {
            await UniTask.NextFrame();
            await SceneManager.LoadSceneAsync(startSceneIndex);
        }

        private void HandleSetting()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeInOutEvent.Initializer(() =>
            {
                uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Setting));
            }));
        }

        private void HandleQuit()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(false,
                () => Application.Quit()));
        }
    }
}
