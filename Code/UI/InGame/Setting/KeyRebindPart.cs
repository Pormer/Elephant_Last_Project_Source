using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame.Setting
{
    [Serializable]
    public struct RebindButton
    {
        public PointerHandler Pointer;
        public DeviceType DeviceType;
    }

    public class KeyRebindPart : MonoBehaviour
    {
        [SerializeField] private RebindButton[] rebindButtons;
        private Dictionary<PointerHandler, DeviceType> _rebindButtons;
        
        public Action<DeviceType> OnRebind;
        private void Awake()
        {
            _rebindButtons = rebindButtons.ToDictionary(k => k.Pointer, v => v.DeviceType);
            foreach (var rb in _rebindButtons)
            {
                rb.Key.Pressed += ()=> OnRebind?.Invoke(rb.Value);
            }
        }

        private void OnDestroy()
        {
            foreach (var rb in _rebindButtons)
            {
                rb.Key.Pressed = null;
            }
        }

        public void Focus() => rebindButtons[0].Pointer.Select();
    }
}