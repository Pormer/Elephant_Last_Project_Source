using System;
using System.Collections.Generic;
using Code.Setting;

namespace Code.UI.InputBind
{
    internal sealed class UIInputKeyCache
    {
        private readonly Dictionary<string, string> _keyboardInputKeys = new();
        private readonly Dictionary<string, string> _gamepadInputKeys = new();

        public Dictionary<string, string> Current(string currentBindingGroup)
        {
            return string.Equals(currentBindingGroup, UIInputBindingGroups.Gamepad, StringComparison.OrdinalIgnoreCase)
                ? _gamepadInputKeys
                : _keyboardInputKeys;
        }

        public Dictionary<string, string> ForDevice(DeviceType deviceType)
        {
            return deviceType switch
            {
                DeviceType.Keyboard => _keyboardInputKeys,
                DeviceType.Xbox or DeviceType.PS => _gamepadInputKeys,
                _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
            };
        }

        public void Update(string key, string keyboardValue, string gamepadValue)
        {
            _keyboardInputKeys[key] = keyboardValue;
            _gamepadInputKeys[key] = gamepadValue;
        }
    }
}
