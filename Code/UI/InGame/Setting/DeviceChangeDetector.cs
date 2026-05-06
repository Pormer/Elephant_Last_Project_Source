using System;
using Code.Setting;
using UnityEngine.InputSystem;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Setting
{
    public class DeviceChangeDetector : IDisposable
    {
        private DeviceType _currentDeviceType;

        public event Action<DeviceType> OnDeviceChanged;

        public void Initialize()
        {
            InputSystem.onActionChange += HandleInputActionChange;

            if (TryGetInitialDeviceType(out var deviceType))
            {
                _currentDeviceType = deviceType;
                OnDeviceChanged?.Invoke(deviceType);
            }
        }

        public void Dispose()
        {
            InputSystem.onActionChange -= HandleInputActionChange;
        }

        private void HandleInputActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed || obj is not InputAction action)
                return;

            var device = action.activeControl?.device;
            if (device == null)
                return;

            if (!TryGetDeviceType(device, out var deviceType))
                return;

            if (_currentDeviceType == deviceType)
                return;

            _currentDeviceType = deviceType;
            OnDeviceChanged?.Invoke(deviceType);
        }

        private static bool TryGetDeviceType(InputDevice device, out DeviceType deviceType)
        {
            if (device is Keyboard || device is Mouse)
            {
                deviceType = DeviceType.Keyboard;
                return true;
            }

            if (device is Gamepad gamepad)
            {
                var layout = gamepad.layout;
                if (!string.IsNullOrWhiteSpace(layout) &&
                    layout.Contains("Dual", StringComparison.OrdinalIgnoreCase))
                {
                    deviceType = DeviceType.PS;
                    return true;
                }

                deviceType = DeviceType.Xbox;
                return true;
            }

            deviceType = default;
            return false;
        }

        private static bool TryGetInitialDeviceType(out DeviceType deviceType)
        {
            if (Gamepad.current != null && TryGetDeviceType(Gamepad.current, out deviceType))
                return true;

            if (Keyboard.current != null || Mouse.current != null)
            {
                deviceType = DeviceType.Keyboard;
                return true;
            }

            deviceType = default;
            return false;
        }
    }
}
