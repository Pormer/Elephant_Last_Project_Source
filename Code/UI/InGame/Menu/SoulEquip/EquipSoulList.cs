using System;
using System.Collections.Generic;
using System.Linq;
using Code.Souls.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class EquipSoulList : UISection, IUIBindable
    {
        private const string LogPrefix = "[CurrentSoulSection]";

        [SerializeField] private EquipSoulItem[] currentSoul;

        private SoulType _currentSoulType = SoulType.God;
        private Dictionary<SoulType, EquipSoulItem> _soulItems;

        public Enum BindKey => SoulUIProperty.CurrentSoulSlot;

        public override Selectable FirstSelectable
        {
            get
            {
                if(_soulItems == null || !_soulItems.TryGetValue(_currentSoulType, out var item))  return null;
                return item.PointerHandler;
            }
        }

        public Action OnPressed;
        public Action<SoulDataSO, SoulType> OnFocus;

        protected override void Initialize()
        {
            base.Initialize();
            _soulItems = currentSoul.ToDictionary(x => x.SoulType);
        }

        public override void InitializeView()
        {
            base.InitializeView();
            foreach (var soul in currentSoul)
            {
                soul.OnPressed += HandlePressed;
                soul.OnFocus += HandleFocus;
            }
        }

        private void OnDestroy()
        {
            foreach (var soul in currentSoul)
            {
                soul.OnPressed -= HandlePressed;
                soul.OnFocus -= HandleFocus;
            }
        }

        private void HandleFocus(SoulDataSO obj, SoulType soulType)
        {
            _currentSoulType  = soulType;
            OnFocus?.Invoke(obj, soulType);
        }

        private void HandlePressed()
        {
            OnPressed?.Invoke();
        }

        public void Bind(object v)
        {
            if (v is not Dictionary<SoulType, SoulDataSO> value) return;
            SetCurrentSouls(value);
        }

        public void SetCurrentSouls(Dictionary<SoulType, SoulDataSO> datas)
        {
            if (datas.Count > currentSoul.Length)
            {
                Debug.LogWarning(
                    $"{LogPrefix} Current soul length is {currentSoul.Length} but data length is {datas.Count}");
                return;
            }

            foreach (var si in _soulItems)
            {
                if(datas.TryGetValue(si.Key, out var v))
                    si.Value.SetInfo(v);
                else
                    si.Value.SetInfo(null);
            }
        }
    }
}