using System;
using System.Collections.Generic;
using System.Linq;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.InGame.CustomElements
{
    [RequireComponent(typeof(UIViewElement))]
    public abstract class CustomDropdown<T> : TMP_Dropdown where T : Enum
    {
        [Serializable]
        private class DropdownOption
        {
            public T option;
            public string text;
        }

        [SerializeField] private List<DropdownOption> items;
        private UIViewElement _viewElement;

        private const string FocusKey = "focus";
        private bool _isExpanded;

        public Action<T> OnValueChanged;

        public T Value { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            _viewElement = GetComponent<UIViewElement>();
            if (items != null)
            {
                items.Sort((x, y) => x.option.CompareTo(y.option));
                ClearOptions();
                AddOptions(items.Select(option => option.text).ToList());
            }

            onValueChanged.AddListener(HandleValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void HandleValueChanged(int idx)
        {
            if (idx < 0 || idx >= items.Count)
                return;

            Value = items[idx].option;
            OnValueChanged?.Invoke(items[idx].option);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _viewElement.AddState(FocusKey).Forget();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (_isExpanded)
                return;

            _viewElement.RemoveState(FocusKey).Forget();
        }

        protected override GameObject CreateDropdownList(GameObject template)
        {
            _isExpanded = true;
            HandleExpand(true);
            _viewElement.AddState(FocusKey).Forget();
            return base.CreateDropdownList(template);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            base.DestroyDropdownList(dropdownList);
            _isExpanded = false;
            HandleExpand(false);
            //
            // if (EventSystem.current?.currentSelectedGameObject != gameObject)
            //     viewElement.RemoveState(FocusKey).Forget();
        }

        public virtual void HandleExpand(bool expanded)
        {
        }

        protected override void OnDisable()
        {
            _isExpanded = false;
            if (_viewElement != null)
                _viewElement?.RemoveState(FocusKey).Forget();
            base.OnDisable();
        }
    }
}