using System;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InputBind
{
    public enum UIInputType
    {
        Cancel,
        Submit,
        Info,
        TabLeft,
        TabRight,
    }


    [CreateAssetMenu(fileName = "UIInputBindSO", menuName = "SO/UI/InputBindSO")]
    public class UIInputBindSO : ScriptableObject
    {
        [SerializeField] private InputActionAsset inputAsset;
        [SerializeField] private GameEventChannelSO settingChannel;

        private bool _isInitialized;
        private bool _didDeviceSetting;
        private string _currentBindingGroup = UIInputBindingGroups.Keyboard;
        private Dictionary<string, InputAction> _input;
        private readonly UIInputKeyCache _keyCache = new();

        private InputActionMap _uiInputMap;
        private InputActionMap _playerInputMap;
        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        public bool IsRebinding { get; private set; }

        public Action OnKeyChanged;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            if (!inputAsset)
            {
                Debug.Log($"Is input asset null : {inputAsset == null}");
                return;
            }

            InputBindingOverrideStore.Register(inputAsset);
            settingChannel.AddListener<DeviceChangeEvent>(HandleDeviceChange);

            _uiInputMap = inputAsset.FindActionMap(UIInputBindingGroups.UIActionMap);
            _playerInputMap = inputAsset.FindActionMap(UIInputBindingGroups.PlayerActionMap);

            _input = new Dictionary<string, InputAction>();
            foreach (UIInputType value in Enum.GetValues(typeof(UIInputType)))
            {
                _input.Add(value.ToString(), _uiInputMap.FindAction(value.ToString()));
            }

            _isInitialized = true;
            UpdateInputDict();
        }

        public void Dispose()
        {
            if (!_isInitialized)
                return;

            CancelRebind();
            settingChannel.RemoveListener<DeviceChangeEvent>(HandleDeviceChange);
            _isInitialized = false;
        }

        private void UpdateInputDict()
        {
            foreach (var input in _input)
            {
                if (input.Key.Contains(UIInputBindingGroups.IndexDivider))
                {
                    var loc = input.Key.IndexOf(UIInputBindingGroups.IndexDivider, StringComparison.Ordinal)
                              + UIInputBindingGroups.IndexDivider.Length;
                    int index = int.Parse(input.Key.Substring(loc));
                    UpdateCache(input.Key, input.Value, index);
                }
                else
                {
                    UpdateCache(input.Key, input.Value);
                }
            }

            OnKeyChanged?.Invoke();
        }

        private void UpdateCache(string key, InputAction action, int index = -1)
        {
            _keyCache.Update(
                key,
                UIInputBindingTextResolver.GetBindingText(action, UIInputBindingGroups.Keyboard, index),
                UIInputBindingTextResolver.GetBindingText(action, UIInputBindingGroups.Gamepad, index));
        }

        private void RefreshBindingCache(string actionName, InputAction action, int bindingDisplayIndex)
        {
            string cacheKey = actionName;
            if (bindingDisplayIndex > -1)
            {
                cacheKey += UIInputBindingGroups.IndexDivider + bindingDisplayIndex;
            }

            UpdateCache(cacheKey, action, bindingDisplayIndex);
            OnKeyChanged?.Invoke();
        }

        private void HandleDeviceChange(DeviceChangeEvent obj)
        {
            SetDeviceType(obj.DeviceType);
            if (!_didDeviceSetting) _didDeviceSetting = true;
        }

        private void SetDeviceType(DeviceType deviceType)
        {
            _currentBindingGroup = deviceType switch
            {
                DeviceType.Keyboard => UIInputBindingGroups.Keyboard,
                DeviceType.Xbox or DeviceType.PS => UIInputBindingGroups.Gamepad,
                _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
            };
            UpdateInputDict();
        }

        public string GetUIKey(UIInputType key)
        {
            if (GetCurrentInputKeys().Count == 0) Initialize();
            return GetCurrentInputKeys().GetValueOrDefault(key.ToString());
        }

        public string GetUIKey(string key, int index = -1)
        {
            if (_input == null)
            {
                Initialize();
            }

            string realKey = key;
            if (index > -1) realKey += UIInputBindingGroups.IndexDivider + index;
            var inputKeys = GetCurrentInputKeys();
            if (inputKeys.TryGetValue(realKey, out string result)) return result;

            if (!_input.TryGetValue(key, out var act))
            {
                act = GetOrCreateAction(_uiInputMap, key);
                Debug.Log($"Does act for {key} exists? : {act == null}");
            }

            var txt = UIInputBindingTextResolver.GetBindingText(act, _currentBindingGroup, index);
            inputKeys[realKey] = txt;

            return txt;
        }

        public string GetPlayerKey(string key, int index = -1)
        {
            if (_input == null)
            {
                Initialize();
            }

            string realKey = key;
            if (index > -1) realKey += UIInputBindingGroups.IndexDivider + index;
            var inputKeys = GetCurrentInputKeys();
            if (inputKeys.TryGetValue(realKey, out string result)) return result;

            if (!_input.TryGetValue(key, out var act))
            {
                act = GetOrCreateAction(_playerInputMap, key);
            }

            var txt = UIInputBindingTextResolver.GetBindingText(act, _currentBindingGroup, index);
            inputKeys[realKey] = txt;

            return txt;
        }

        public string GetPlayerKey(string key, DeviceType deviceType, int index = -1)
        {
            if (_input == null)
            {
                Initialize();
            }

            string realKey = key;
            if (index > -1) realKey += UIInputBindingGroups.IndexDivider + index;
            var inputKeys = GetInputKeys(deviceType);
            if (inputKeys.TryGetValue(realKey, out string result)) return result;

            if (!_input.TryGetValue(key, out var act))
            {
                act = GetOrCreateAction(_playerInputMap, key);
            }

            var txt = UIInputBindingTextResolver.GetBindingText(act, GetBindingGroup(deviceType), index);
            inputKeys[realKey] = txt;

            return txt;
        }

        public bool StartUIRebind(UIInputType key, DeviceType deviceType, int index = -1)
        {
            Initialize();
            IsRebinding = true;
            return StartRebind(_uiInputMap, key.ToString(), deviceType, index);
        }

        public bool StartPlayerRebind(string actionName, DeviceType deviceType, int index = -1)
        {
            Initialize();
            IsRebinding = true;
            return StartRebind(_playerInputMap, actionName, deviceType, index);
        }

        public void ResetUIBinding(UIInputType key, DeviceType deviceType, int index = -1)
        {
            Initialize();
            ResetBinding(_uiInputMap, key.ToString(), deviceType, index);
        }

        public void ResetPlayerBinding(string actionName, DeviceType deviceType, int index = -1)
        {
            Initialize();
            ResetBinding(_playerInputMap, actionName, deviceType, index);
        }

        public void CancelRebind()
        {
            if (_rebindingOperation == null)
                return;

            _rebindingOperation.Cancel();
        }

        private Dictionary<string, string> GetCurrentInputKeys()
        {
            return _keyCache.Current(_currentBindingGroup);
        }

        private Dictionary<string, string> GetInputKeys(DeviceType deviceType)
        {
            return _keyCache.ForDevice(deviceType);
        }

        private static string GetBindingGroup(DeviceType deviceType)
        {
            return deviceType switch
            {
                DeviceType.Keyboard => UIInputBindingGroups.Keyboard,
                DeviceType.Xbox or DeviceType.PS => UIInputBindingGroups.Gamepad,
                _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
            };
        }

        private bool StartRebind(InputActionMap actionMap, string actionName, DeviceType deviceType,
            int bindingDisplayIndex)
        {
            if (_rebindingOperation != null)
                return false;

            var action = GetOrCreateAction(actionMap, actionName);
            if (action == null)
                return false;

            int bindingIndex = FindBindingIndex(action, deviceType, bindingDisplayIndex);
            if (bindingIndex < 0)
            {
                Debug.LogWarning(
                    $"Cannot find binding index. Action : {actionName}, Device : {deviceType}, DisplayIndex : {bindingDisplayIndex}");
                return false;
            }

            var currentMap = action.actionMap;
            bool wasEnabled = currentMap != null && currentMap.enabled;
            if (wasEnabled)
            {
                currentMap.Disable();
            }

            _rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithCancelingThrough("<Gamepad>/start")
                .OnCancel(operation =>
                {
                    CompleteRebind(operation, wasEnabled, false, actionName, bindingDisplayIndex);
                })
                .OnComplete(operation =>
                {
                    CompleteRebind(operation, wasEnabled, true, actionName, bindingDisplayIndex);
                });

            var expectedPath = GetRebindDevicePath(deviceType);
            if (!string.IsNullOrWhiteSpace(expectedPath))
            {
                _rebindingOperation.WithControlsHavingToMatchPath(expectedPath);
            }

            ConfigureExcludedControls(action, bindingIndex, _rebindingOperation, deviceType);
            _rebindingOperation.Start();

            return true;
        }

        private void ResetBinding(InputActionMap actionMap, string actionName, DeviceType deviceType,
            int bindingDisplayIndex)
        {
            var action = GetOrCreateAction(actionMap, actionName);
            if (action == null)
                return;

            int bindingIndex = FindBindingIndex(action, deviceType, bindingDisplayIndex);
            if (bindingIndex < 0)
                return;

            action.RemoveBindingOverride(bindingIndex);
            InputBindingOverrideStore.Save(inputAsset);
            RefreshBindingCache(actionName, action, bindingDisplayIndex);
        }

        private void CompleteRebind(InputActionRebindingExtensions.RebindingOperation operation, bool wasEnabled,
            bool saveOverride, string actionName, int bindingDisplayIndex)
        {
            var action = operation.action;
            var actionMap = operation.action?.actionMap;
            operation.Dispose();
            _rebindingOperation = null;

            if (saveOverride)
            {
                InputBindingOverrideStore.Save(inputAsset);
            }

            if (wasEnabled && actionMap != null)
            {
                actionMap.Enable();
            }


            IsRebinding = false;
            if (action != null)
            {
                RefreshBindingCache(actionName, action, bindingDisplayIndex);
            }
        }

        private InputAction GetOrCreateAction(InputActionMap actionMap, string actionName)
        {
            if (_input.TryGetValue(actionName, out var cachedAction))
                return cachedAction;

            var action = actionMap?.FindAction(actionName);
            if (action != null)
            {
                _input[actionName] = action;
            }

            return action;
        }

        private static int FindBindingIndex(InputAction action, DeviceType deviceType, int bindingDisplayIndex)
        {
            int targetDisplayIndex = Mathf.Max(0, bindingDisplayIndex);
            int currentIndex = 0;
            string bindingGroup = GetBindingGroup(deviceType);

            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite)
                    continue;

                if (!IsBindingMatch(binding, bindingGroup))
                    continue;

                if (currentIndex == targetDisplayIndex)
                    return i;

                currentIndex++;
            }

            return -1;
        }

        private static bool IsBindingMatch(InputBinding binding, string bindingGroup)
        {
            return HasGroup(binding, bindingGroup) || MatchesDevicePath(binding, bindingGroup);
        }

        private static bool HasGroup(InputBinding binding, string bindingGroup)
        {
            if (string.IsNullOrWhiteSpace(binding.groups))
                return false;

            var groups = binding.groups.Split(';');
            for (int i = 0; i < groups.Length; i++)
            {
                if (string.Equals(groups[i], bindingGroup, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesDevicePath(InputBinding binding, string bindingGroup)
        {
            var path = binding.effectivePath;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = binding.path;
            }

            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (string.Equals(bindingGroup, UIInputBindingGroups.Gamepad, StringComparison.OrdinalIgnoreCase))
            {
                return path.Contains("<Gamepad>", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(bindingGroup, UIInputBindingGroups.Keyboard, StringComparison.OrdinalIgnoreCase))
            {
                return path.Contains("<Keyboard>", StringComparison.OrdinalIgnoreCase) ||
                       path.Contains("<Mouse>", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private static string GetRebindDevicePath(DeviceType deviceType)
        {
            return deviceType == DeviceType.Keyboard ? "<Keyboard>" : "<Gamepad>";
        }

        //나중에 수정해야함..................
        private void ConfigureExcludedControls(InputAction targetAction, int index,
            InputActionRebindingExtensions.RebindingOperation operation,
            DeviceType deviceType)
        {
            #region 나중에 고칠거(스압)

            operation
                .WithControlsExcluding("<Pointer>")
                .WithControlsExcluding("<Mouse>")
                .WithControlsExcluding("<Touchscreen>")
                .WithControlsExcluding("<Pen>")
                .WithControlsExcluding("<XRController>")
                .WithControlsExcluding("<Keyboard>/anyKey")
                .WithControlsExcluding("<Keyboard>/backquote")
                .WithControlsExcluding("<Keyboard>/quote")
                .WithControlsExcluding("<Keyboard>/period")
                .WithControlsExcluding("<Keyboard>/slash")
                .WithControlsExcluding("<Keyboard>/backslash")
                .WithControlsExcluding("<Keyboard>/leftBracket")
                .WithControlsExcluding("<Keyboard>/rightBracket")
                .WithControlsExcluding("<Keyboard>/minus")
                .WithControlsExcluding("<Keyboard>/equals")
                .WithControlsExcluding("<Keyboard>/insert")
                .WithControlsExcluding("<Keyboard>/delete")
                .WithControlsExcluding("<Keyboard>/leftMeta")
                .WithControlsExcluding("<Keyboard>/rightMeta")
                .WithControlsExcluding("<Keyboard>/contextMenu")
                .WithControlsExcluding("<Keyboard>/pageDown")
                .WithControlsExcluding("<Keyboard>/pageUp")
                .WithControlsExcluding("<Keyboard>/home")
                .WithControlsExcluding("<Keyboard>/end")
                .WithControlsExcluding("<Keyboard>/capsLock")
                .WithControlsExcluding("<Keyboard>/numLock")
                .WithControlsExcluding("<Keyboard>/printScreen")
                .WithControlsExcluding("<Keyboard>/scrollLock")
                .WithControlsExcluding("<Keyboard>/pause")
                .WithControlsExcluding("<Keyboard>/numpadEnter")
                .WithControlsExcluding("<Keyboard>/numpadDivide")
                .WithControlsExcluding("<Keyboard>/numpadMultiply")
                .WithControlsExcluding("<Keyboard>/numpadPlus")
                .WithControlsExcluding("<Keyboard>/numpadMinus")
                .WithControlsExcluding("<Keyboard>/numpadPeriod")
                .WithControlsExcluding("<Keyboard>/numpadEquals")
                .WithControlsExcluding("<Keyboard>/numpad1")
                .WithControlsExcluding("<Keyboard>/numpad2")
                .WithControlsExcluding("<Keyboard>/numpad3")
                .WithControlsExcluding("<Keyboard>/numpad4")
                .WithControlsExcluding("<Keyboard>/numpad5")
                .WithControlsExcluding("<Keyboard>/numpad6")
                .WithControlsExcluding("<Keyboard>/numpad7")
                .WithControlsExcluding("<Keyboard>/numpad8")
                .WithControlsExcluding("<Keyboard>/numpad9")
                .WithControlsExcluding("<Keyboard>/numpad0")
                .WithControlsExcluding("<Keyboard>/escape")
                .WithControlsExcluding("<Keyboard>/tab")
                .WithControlsExcluding("<Keyboard>/enter")
                .WithControlsExcluding("<Keyboard>/comma")
                .WithControlsExcluding("<Keyboard>/semicolon")
                .WithControlsExcluding("<Gamepad>/start");

            #endregion

            foreach (var input in _input.Values)
            {
                for (var i = 0; i < input.bindings.Count; i++)
                {
                    if (input.actionMap != targetAction.actionMap) continue;
                    if (targetAction == input && index == i) continue;
                    var bindings = input.bindings[i];
                    operation.WithControlsExcluding(bindings.effectivePath);
                }
            }

            switch (deviceType)
            {
                case DeviceType.Keyboard:
                    operation
                        .WithControlsExcluding("<Gamepad>");
                    break;

                case DeviceType.Xbox:
                case DeviceType.PS:
                    operation
                        .WithControlsExcluding("<Keyboard>");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null);
            }
        }

#if UNITY_EDITOR
        [Header("DEBUG")] [SerializeField] private UIInputType inputType;

        [ContextMenu("Set to keyboard")]
        public void SetKeyboard() => SetDeviceType(DeviceType.Keyboard);

        [ContextMenu("Set to gamepad")]
        public void SetGamepad() => SetDeviceType(DeviceType.Xbox);

        [ContextMenu("Get target key")]
        public void GetTargetKey()
        {
            Debug.Log(GetUIKey(inputType));
        }
#endif
    }
}