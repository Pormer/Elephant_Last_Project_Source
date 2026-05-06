using System;
using Code.UI.InputBind;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using Input;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Setting.KeySetting
{
    public class RebindingControl : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UIViewElement element;
        [SerializeField] private UIInputBindSO bindSo;
        [SerializeField] private UIInputSO input;
        [SerializeField] private DeviceType deviceType;
        [SerializeField] private Selectable empty;
        public DeviceType DeviceType => deviceType;
        private BindItem[] _bindItems;
        
        private BindItem _lastBindItem;

        public bool IsShowing { get; private set; }
        private const string ShowKey = "show";
        
        public Action OnBindingComplete;

        private void Awake()
        {
            _bindItems = GetComponentsInChildren<BindItem>();
            foreach (var bindItem in _bindItems)
            {
                bindItem.Initialize(bindSo, deviceType);
                bindItem.OnBinding += HandleBinding;
                bindItem.OnBindItemClick += HandleBindingStart;
            }

            input.OnCancelPressed += HandleBindingComplete;
        }

        private void OnDestroy()
        {
            foreach (var bindItem in _bindItems)
            {
                bindItem.OnBinding -= HandleBinding;
                bindItem.OnBindItemClick -= HandleBindingStart;
            }
            input.OnCancelPressed -= HandleBindingComplete;
        }

        private void HandleBindingStart(BindItem obj) => _lastBindItem = obj;

        private void HandleBinding(bool obj)
        {
            input.SetEnabled(!obj);
            foreach (var bindItem in _bindItems)
            {
                bindItem.PointerHandler.interactable = !obj;
            }

            if (!obj)
                _lastBindItem.PointerHandler.Select();
            else
                empty.Select();
        }

        private void HandleBindingComplete()
        {
            if (bindSo.IsRebinding) return;
            OnBindingComplete?.Invoke();
            Hide();
        }

        public async void Show()
        {
            IsShowing = true;
            if (canvasGroup)
                canvasGroup.interactable = true;
            await element.AddState(ShowKey);
            _bindItems[0].PointerHandler.Select();
        }

        public async void Hide()
        {
            if (canvasGroup)
                canvasGroup.interactable = false;
            await element.RemoveState(ShowKey);
            IsShowing = false;
        }
    }
}