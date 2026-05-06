using System;
using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu
{
    public class MenuTab : MonoBehaviour
    {
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private UIViewElement element;
        [SerializeField] private UIStateType tabState;
        [SerializeField] private GameEventChannelSO uiChannel;

        private const string OpenKey = "open";

        private void Awake()
        {
            pointerHandler.Pressed += HandleTabPressed;
            uiChannel.AddListener<SubUIChangeEvent>(HandleUIChange);
        }

        private void OnDestroy()
        {
            pointerHandler.Pressed -= HandleTabPressed;
            uiChannel.RemoveListener<SubUIChangeEvent>(HandleUIChange);
        }

        private void HandleUIChange(SubUIChangeEvent obj)
        {
            if (obj.NewStateType == tabState)
                element.AddState(OpenKey, 10).Forget();
            else
                element.RemoveState(OpenKey).Forget();

            RefreshNavigationNextFrame().Forget();
        }

        private void HandleTabPressed()
        {
            uiChannel.RaiseEvent(UIEvents.SubUIChangeEvent.Initializer(tabState));
        }

        private async UniTaskVoid RefreshNavigationNextFrame()
        {
            await UniTask.WaitForEndOfFrame(this);
            if (pointerHandler == null) return;
        }
    }
}
