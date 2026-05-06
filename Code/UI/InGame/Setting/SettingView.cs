using System;
using System.Collections.Generic;
using System.Linq;
using Code.Setting;
using Code.UI.InGame.Setting.Elements;
using Code.UI.InGame.Setting.KeySetting;
using Code.UI.Interface;
using Code.UI.Visual;
using Input;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Setting
{
    public abstract class SettingView : ViewBase, ISelectableState
    {
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected UIViewElement element;
        [SerializeField] protected UIInputSO uiInput;
        [SerializeField] protected Selectable defaultFocusable;
        [SerializeField] protected DisplayConfirmor displayConfirmor;
        [SerializeField] protected RebindingControl[] rebindingControls;

        [Header("Parts")] [SerializeField] private VolumePart volumePart;
        [SerializeField] private LanguageDropdown languagePart;
        [SerializeField] private DisplayPart displayPart;
        [SerializeField] private KeyRebindPart keyRebindPart;

        private ISettingElement[] _settingElements;
        private Selectable[] _bindItems;

        protected bool IsCanceling = false;

        private Dictionary<DeviceType, RebindingControl> _rebindingControls;

        public Action<VolumeType, float> OnVolumeChanged;

        public Action OnRevertDisplay;
        public Action OnRevertScreen;
        public Action<bool, ResolutionType> OnApplyDisplay;
        public Action<LanguageType> OnLanguageChange;

        public Selectable DefaultFocusable => defaultFocusable;
        protected bool IsKeyRebinding => rebindingControls.Any(x => x.IsShowing);

        private void Awake()
        {
            _rebindingControls = rebindingControls.ToDictionary(x => x.DeviceType, x => x);

            SubscribePartEvents();

            _settingElements = GetComponentsInChildren<ISettingElement>();
            SubscribeSettingElementEvents();
        }

        private void OnDestroy()
        {
            UnsubscribePartEvents();
            UnsubscribeSettingElementEvents();
        }

        private async void HandleDisplayApply(bool fullScreen, ResolutionType resolution)
        {
            HandleSubElementFocused(true);
            var confirm = await displayConfirmor.ApplySetting(fullScreen, resolution);
            HandleSubElementFocused(false);
            displayPart.ApplyButton.Select();

            if (confirm)
                OnApplyDisplay?.Invoke(fullScreen, resolution);
            else
                OnRevertScreen?.Invoke();
        }

        private void HandleDisplayRevert()
        {
            OnRevertDisplay?.Invoke();
        }

        protected override List<IUIBindable> InitializeBindables()
        {
            return new List<IUIBindable>
            {
                volumePart,
                displayPart
            };
        }

        protected virtual void HandleSubElementFocused(bool obj)
        {
        }

        private void HandleVolumeChanged(VolumeType volume, float value) => OnVolumeChanged?.Invoke(volume, value);

        private void HandleRebind(DeviceType obj)
        {
            if(IsCanceling) return;
            Debug.Log($"Ow...");

            HandleSubElementFocused(true);
            _rebindingControls[obj].Show();
            _rebindingControls[obj].OnBindingComplete += HandleBindingComplete;
        }

        private void HandleBindingComplete()
        {
            HandleSubElementFocused(false);
            keyRebindPart.Focus();
        }

        public void SetDisplaySetting(bool fullScreen, ResolutionType resolution) =>
            displayPart.SetValues(fullScreen, resolution);

        private void SubscribePartEvents()
        {
            volumePart.OnValueChanged += HandleVolumeChanged;
            displayPart.OnApplySetting += HandleDisplayApply;
            displayPart.OnRevertSetting += HandleDisplayRevert;
            keyRebindPart.OnRebind += HandleRebind;
            languagePart.OnValueChanged += HandleLanguageChange;
        }

        private void UnsubscribePartEvents()
        {
            volumePart.OnValueChanged -= HandleVolumeChanged;
            displayPart.OnApplySetting -= HandleDisplayApply;
            displayPart.OnRevertSetting -= HandleDisplayRevert;
            keyRebindPart.OnRebind -= HandleRebind;
            languagePart.OnValueChanged -= HandleLanguageChange;
        }

        private void HandleLanguageChange(LanguageType obj)
        {
            OnLanguageChange?.Invoke(obj);
        }

        private void SubscribeSettingElementEvents()
        {
            if (_settingElements == null) return;

            foreach (var settingElement in _settingElements)
                settingElement.OnSubElementFocus += HandleSubElementFocused;
        }

        private void UnsubscribeSettingElementEvents()
        {
            if (_settingElements == null) return;

            foreach (var settingElement in _settingElements)
                settingElement.OnSubElementFocus -= HandleSubElementFocused;
        }
    }
}