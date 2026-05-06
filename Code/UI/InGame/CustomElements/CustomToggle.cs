using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.InGame.CustomElements
{
    public class CustomToggle : Toggle
    {
        private UIViewElement _element;
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
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _element.RemoveState(FocusKey).Forget();
        }
    }
}