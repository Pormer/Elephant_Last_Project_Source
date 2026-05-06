using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    [RequireComponent(typeof(UIViewElement))]
    public class PointerHandler : Selectable, ISubmitHandler
    {
        [field: SerializeField] public bool CanBePressed { get; set; } = true;
        private UIViewElement _viewElement;
        private bool _isFocused;
        private bool _isPressed;
        private bool _isInactive;

        public Action Pressed;
        public Action Focused;
        public Action Defocused;

        protected override void Awake()
        {
            base.Awake();
            _viewElement = GetComponent<UIViewElement>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    SetFocus(false);
                    SetInteractable(true);
                    SetPressed(false).Forget();
                    break;
                case SelectionState.Highlighted:
                        Select();
                    break;
                case SelectionState.Pressed:
                    if (CanBePressed)
                        SetPressed(true).Forget();
                    break;
                case SelectionState.Selected:
                    SetFocus(true);
                    SetPressed(false).Forget();
                    break;
                case SelectionState.Disabled:
                    SetFocus(false);
                    SetInteractable(false);
                    // SetPressed(false).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        // public virtual void OnPointerEnter(PointerEventData eventData) => SetFocus(true);
        // public virtual void OnPointerExit(PointerEventData eventData) => target.RemoveState("hover").Forget();

        private void SetFocus(bool focus, bool withEvent = true)
        {
            if (_viewElement == null) return;
            if (_isFocused == focus) return;
            _isFocused = focus;

            if (focus)
            {
                _viewElement.AddState("focus", 5).Forget();
                if (withEvent)
                    Focused?.Invoke();
            }
            else
            {
                _viewElement.RemoveState("focus").Forget();
                if (withEvent)
                    Defocused?.Invoke();
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            if (_viewElement == null) return;
            var shouldBeInactive = !isInteractable;
            if (_isInactive == shouldBeInactive) return;
            _isInactive = shouldBeInactive;

            if (isInteractable)
            {
                _viewElement.RemoveState("inactive").Forget();
            }
            else
            {
                _viewElement.AddState("inactive", 2).Forget();
            }
        }

        private async UniTask SetPressed(bool isPressed, bool withEvent = true)
        {
            if (_viewElement == null) return;
            if (_isPressed == isPressed) return;
            _isPressed = isPressed;

            if (isPressed)
            {
                if (withEvent)
                    Pressed?.Invoke();
                await _viewElement.AddState("press", 7);
            }
            else
            {
                await _viewElement.RemoveState("press");
            }
        }

        public async void OnSubmit(BaseEventData eventData)
        {
            if(!CanBePressed)  return;
            await SetPressed(true);
            await SetPressed(false);
        }
    }
}
