using System;
using System.Collections.Generic;
using Code.Setting;
using Code.UI.InGame.CustomElements;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame.Setting.Elements
{
    public class VolumeBar : MonoBehaviour, ISettingElement
    {
        [SerializeField] public VolumeType volumeType;
        [SerializeField] private CustomSlider slider;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private UIViewElement element;
        
        public Action<VolumeType, float> OnValueChanged;
        public Action<bool> OnSubElementFocus { get; set; }
        public const string SubFocusKey = "sub-focus";

        protected void Awake()
        {
            pointerHandler.Pressed += HandlePressed;
            slider.OnDefocus += HandleSliderDefocus;
            slider.onValueChanged.AddListener(HandleValueChange);
        }

        protected void OnDestroy()
        {
            pointerHandler.Pressed -= HandlePressed;
            slider.OnDefocus -= HandleSliderDefocus;
            slider.onValueChanged.RemoveListener(HandleValueChange);
        }

        private void HandleSliderDefocus()
        {
            pointerHandler.Select();
            element.RemoveState(SubFocusKey).Forget();
            OnSubElementFocus?.Invoke(false);
        }

        private void HandlePressed()
        {
            slider.Select();
            element.AddState(SubFocusKey).Forget();
            OnSubElementFocus?.Invoke(true);
        }

        private void HandleValueChange(float v)
        {
            OnValueChanged?.Invoke(volumeType, v/slider.maxValue);
            valueText.text = ((Mathf.RoundToInt(v) * 10).ToString());
        }

        public void SetVolume(float v)
        {
            int value = (int)(v * slider.maxValue);
            slider.SetValueWithoutNotify(value);
            valueText.text = ((Mathf.RoundToInt(value) * 10).ToString());
        }
    }
}