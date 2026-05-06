using System;
using System.Collections.Generic;
using System.Linq;
using EventSystem;
using UnityEngine;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InputBind
{
    [CreateAssetMenu(fileName = "KeyIconData", menuName = "SO/UI/KeyIconData", order = 0)]
    public class KeyIconDataSO : ScriptableObject
    {
        [SerializeField] private GameEventChannelSO settingChannel;
        [SerializeField] private List<IconData> keyboardIcons = new();
        [SerializeField] private List<IconData> xboxIcons = new();
        [SerializeField] private List<IconData> psIcons = new();

        private DeviceType _deviceType;
        private bool _isInitialized;

        public Dictionary<string, Sprite> KeyboardIcons;
        public Dictionary<string, Sprite> XboxIcons;
        public Dictionary<string, Sprite> PSIcons;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            KeyboardIcons = keyboardIcons.ToDictionary(k => k.keyName.ToLower(), v => v.sprite);
            XboxIcons = xboxIcons.ToDictionary(k => k.keyName.ToLower(), v => v.sprite);
            PSIcons = psIcons.ToDictionary(k => k.keyName.ToLower(), v => v.sprite);
            settingChannel?.AddListener<DeviceChangeEvent>(HandleDeviceChange);
            _isInitialized = true;
        }

        public void Dispose()
        {
            if (!_isInitialized)
                return;

            settingChannel?.RemoveListener<DeviceChangeEvent>(HandleDeviceChange);
            _isInitialized = false;
        }

        private void HandleDeviceChange(DeviceChangeEvent obj)
        {
            _deviceType = obj.DeviceType;
        }

        public Sprite GetIcon(string k)
        {
            return GetIcon(k, _deviceType);
        }

        public Sprite GetIcon(string k, DeviceType deviceType)
        {
            if (!_isInitialized || KeyboardIcons == null || XboxIcons == null || PSIcons == null)
            {
                Initialize();
            }

            if (string.IsNullOrWhiteSpace(k))
                return null;

            k = k.ToLower();
            var result = deviceType switch
            {
                DeviceType.Keyboard => KeyboardIcons.GetValueOrDefault(k),
                DeviceType.Xbox => XboxIcons.GetValueOrDefault(k),
                DeviceType.PS => PSIcons.GetValueOrDefault(k),
                _ => throw new ArgumentOutOfRangeException()
            };
            return result;
        }
    }

    [Serializable]
    public class IconData
    {
        public string keyName;
        public Sprite sprite;
    }
}
