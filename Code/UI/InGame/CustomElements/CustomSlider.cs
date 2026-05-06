using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.InGame.CustomElements
{
    [RequireComponent(typeof(UIViewElement))]
    public class CustomSlider : Slider, ICancelHandler, ISubmitHandler
    {
        private UIViewElement _element;
        public Action OnFocus;
        public Action OnDefocus;
        private const string FocusKey = "focus";

        protected override void Awake()
        {
            base.Awake();
            _element = GetComponent<UIViewElement>();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _element.AddState(FocusKey).Forget();
            OnFocus?.Invoke();
        }

        public void OnCancel(BaseEventData eventData)
        {
            _element.RemoveState(FocusKey).Forget();
            OnDefocus?.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            _element.RemoveState(FocusKey).Forget();
            OnDefocus?.Invoke();
        }
    }
}