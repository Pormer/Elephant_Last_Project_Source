using System;
using System.Collections.Generic;
using System.Linq;
using Code.Setting;
using Code.UI.InGame.Models;
using Code.UI.InGame.Setting.Elements;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI.InGame.Setting
{
    public class VolumePart : MonoBehaviour, IUIBindable
    {
        private Dictionary<VolumeType, VolumeBar> _volumeBars;
        public Action<VolumeType, float> OnValueChanged;

        public Enum BindKey => SettingUIProperty.VolumeValues;

        public void Bind(object v)
        {
            if (v is not Dictionary<VolumeType, float> value) return;

            foreach (var volumeBar in value)
            {
                if (_volumeBars.TryGetValue(volumeBar.Key, out var volumeBarValue))
                    volumeBarValue.SetVolume(volumeBar.Value);
            }
        }

        private void Awake()
        {
            _volumeBars = GetComponentsInChildren<VolumeBar>().ToDictionary(x => x.volumeType);
            foreach (var volumeBar in _volumeBars.Values)
            {
                volumeBar.OnValueChanged += HandleValueChange;
            }
        }

        private void OnDestroy()
        {
            foreach (var volumeBar in _volumeBars.Values)
            {
                volumeBar.OnValueChanged -= HandleValueChange;
            }
        }

        private void HandleValueChange(VolumeType vt, float value) => OnValueChanged?.Invoke(vt, value);
    }
}