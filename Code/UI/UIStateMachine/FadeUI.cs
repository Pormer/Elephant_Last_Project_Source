using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using Input;
using UnityEngine;

namespace Code.UI.UIStateMachine
{
    public class FadeUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private UIViewElement fadeUIElement;
        [SerializeField] private bool startVisible = false;
        [SerializeField] private Canvas canvas;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private UIInputSO uiInput;

        private const string ShowKey = "show";
        private const string FadeKey = "fade";

        private void Awake()
        {
            uiEventChannel.AddListener<UIFadeStartEvent>(HandleUIFadeStart);
            uiEventChannel.AddListener<UIFadeInOutEvent>(HandleUIFadeInOut);
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<UIFadeStartEvent>(HandleUIFadeStart);
            uiEventChannel.RemoveListener<UIFadeInOutEvent>(HandleUIFadeInOut);
        }

        private async void Start()
        {
            if (fadeUIElement == null) return;
            if (startVisible)
            {
                await fadeUIElement.AddState(ShowKey, 10);
                await UniTask.NextFrame();
                FadeOut().Forget();
            }
        }

        private async void HandleUIFadeInOut(UIFadeInOutEvent obj)
        {
            await FadeIn(obj.Callback);
            FadeOut().Forget();
        }

        private void HandleUIFadeStart(UIFadeStartEvent obj)
        {
            if (obj.IsFadeIn)
                FadeIn(obj.Callback).Forget();
            else
                FadeOut(obj.Callback).Forget();
        }

        private async UniTask FadeOut(Action callback = null)
        {
            if (fadeUIElement == null) return;
            // fadeUIElement.PlayFeedback(FadeKey);
            await UniTask.WhenAll( fadeUIElement.RemoveState(ShowKey), fadeUIElement.RemoveState(FadeKey));
            if (canvas)
                canvas.enabled = false;
            uiEventChannel.RaiseEvent(UIEvents.UIFadeCompleteEvent.Initializer(false));
            playerInput.SetEnabled(true);
            uiInput.SetEnabled(true);
            if (callback != null)
                callback();
        }

        private async UniTask FadeIn(Action callback = null)
        {
            if (fadeUIElement == null) return;
            if (canvas)
                canvas.enabled = true;
            playerInput.SetEnabled(false);
            uiInput.SetEnabled(false);
            await UniTask.WhenAll(fadeUIElement.AddState(ShowKey, 10, true), fadeUIElement.AddState(FadeKey, 10));
            uiEventChannel.RaiseEvent(UIEvents.UIFadeCompleteEvent.Initializer(true));
            if (callback != null)
                callback();
        }
    }
}