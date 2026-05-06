using System;
using Code.Setting;
using Code.UI.InGame.CustomElements;
using Code.UI.InGame.Models;
using Code.UI.InGame.Setting.Elements;
using Code.UI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Setting
{
    public class DisplayPart : MonoBehaviour, IUIBindable
    {
        [SerializeField] private CustomToggle fullScreenToggle;
        [SerializeField] private DisplayDropdown resolutionDropdown;
        [SerializeField] private PointerHandler applyButton;
        [SerializeField] private PointerHandler revertButton;

        public Action OnRevertSetting;
        public Action<bool, ResolutionType> OnApplySetting;
        
        public Selectable ApplyButton => applyButton;

        private void Awake()
        {
            applyButton.Pressed += HandleApply;
            revertButton.Pressed += HandleRevert;
        }

        private void OnDestroy()
        {
            applyButton.Pressed -= HandleApply;
            revertButton.Pressed -= HandleRevert;
        }

        private void HandleApply()
        {
            OnApplySetting?.Invoke(fullScreenToggle.isOn, resolutionDropdown.Value);
        }

        private void HandleRevert()
        {
            OnRevertSetting?.Invoke();
        }

        public void SetValues(bool fullScreen, ResolutionType value)
        {
            fullScreenToggle.SetIsOnWithoutNotify(fullScreen);
            resolutionDropdown.SetValueWithoutNotify((int)value);
        }

        public Enum BindKey => SettingUIProperty.DisplayDatas;
        public void Bind(object v)
        {
            if (v is not DisplayData displayData) return;
            SetValues(displayData.IsFullScreen, displayData.Resolution);
        }
    }
}