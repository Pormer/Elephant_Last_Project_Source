using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UI.InputBind
{
    internal static class UIInputBindingTextResolver
    {
        public static string GetBindingText(InputAction action, string bindingGroup, int index = -1)
        {
            if (action == null) return "(No Binding)";

            int targetIndex = Mathf.Max(0, index);
            int displayIndex = 0;
            int fallbackIndex = -1;

            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];
                if (binding.isComposite) continue;

                bool groupMatch = HasGroup(binding, bindingGroup);
                bool pathMatch = MatchesDevicePath(binding, bindingGroup);
                if (!groupMatch && !pathMatch) continue;

                if (displayIndex < targetIndex)
                {
                    displayIndex++;
                    continue;
                }

                if (pathMatch && !IsWildcardUsage(binding))
                {
                    var pathDisplay = ResolveBindingPathDisplay(binding, bindingGroup);
                    if (!string.IsNullOrWhiteSpace(pathDisplay))
                    {
                        return pathDisplay.ToLower();
                    }

                    return action.GetBindingDisplayString(i).ToLower();
                }

                if (fallbackIndex < 0)
                {
                    fallbackIndex = i;
                }
            }

            if (fallbackIndex >= 0)
            {
                var resolved = ResolveDisplayFromControls(action, fallbackIndex, bindingGroup);
                if (!string.IsNullOrWhiteSpace(resolved))
                {
                    return resolved.ToLower();
                }

                return action.GetBindingDisplayString(fallbackIndex).ToLower();
            }

            return "(No Binding)";
        }

        private static bool HasGroup(InputBinding binding, string bindingGroup)
        {
            if (string.IsNullOrWhiteSpace(binding.groups)) return false;

            var groups = binding.groups.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var group in groups)
            {
                if (string.Equals(group, bindingGroup, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesDevicePath(InputBinding binding, string bindingGroup)
        {
            var path = binding.effectivePath;
            if (string.IsNullOrWhiteSpace(path)) path = binding.path;
            if (string.IsNullOrWhiteSpace(path)) return false;

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

        private static bool IsWildcardUsage(InputBinding binding)
        {
            var path = binding.effectivePath;
            if (string.IsNullOrWhiteSpace(path)) path = binding.path;
            if (string.IsNullOrWhiteSpace(path)) return false;

            return path.StartsWith("*/{", StringComparison.Ordinal) && path.EndsWith("}", StringComparison.Ordinal);
        }

        private static string ResolveDisplayFromControls(InputAction action, int bindingIndex, string bindingGroup)
        {
            var binding = action.bindings[bindingIndex];
            var pathDisplay = ResolveBindingPathDisplay(binding, bindingGroup);
            if (!string.IsNullOrWhiteSpace(pathDisplay))
            {
                return pathDisplay;
            }

            foreach (var control in action.controls)
            {
                if (control == null || control.device == null) continue;
                if (!IsControlInGroup(control, bindingGroup)) continue;
                if (action.GetBindingIndexForControl(control) != bindingIndex) continue;

                if (!string.IsNullOrWhiteSpace(control.shortDisplayName)) return control.shortDisplayName;
                if (!string.IsNullOrWhiteSpace(control.displayName)) return control.displayName;
                if (!string.IsNullOrWhiteSpace(control.name)) return control.name;
            }

            return null;
        }

        private static string ResolveBindingPathDisplay(InputBinding binding, string bindingGroup)
        {
            var path = binding.effectivePath;
            if (string.IsNullOrWhiteSpace(path)) path = binding.path;
            if (string.IsNullOrWhiteSpace(path)) return null;

            if (IsWildcardUsage(binding))
            {
                const string wildcardPrefix = "*/";
                if (!path.StartsWith(wildcardPrefix, StringComparison.Ordinal)) return null;

                string usagePart = path.Substring(wildcardPrefix.Length);
                if (string.Equals(bindingGroup, UIInputBindingGroups.Gamepad, StringComparison.OrdinalIgnoreCase))
                {
                    path = $"<Gamepad>/{usagePart}";
                }
                else if (string.Equals(bindingGroup, UIInputBindingGroups.Keyboard, StringComparison.OrdinalIgnoreCase))
                {
                    path = $"<Keyboard>/{usagePart}";
                }
            }

            if (string.IsNullOrWhiteSpace(path)) return null;

            var humanReadable = InputControlPath.ToHumanReadableString(path,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            return string.IsNullOrWhiteSpace(humanReadable) ? null : humanReadable;
        }

        private static bool IsControlInGroup(InputControl control, string bindingGroup)
        {
            if (string.Equals(bindingGroup, UIInputBindingGroups.Gamepad, StringComparison.OrdinalIgnoreCase))
            {
                return control.device is Gamepad;
            }

            if (string.Equals(bindingGroup, UIInputBindingGroups.Keyboard, StringComparison.OrdinalIgnoreCase))
            {
                return control.device is Keyboard || control.device is Mouse;
            }

            return false;
        }
    }
}
