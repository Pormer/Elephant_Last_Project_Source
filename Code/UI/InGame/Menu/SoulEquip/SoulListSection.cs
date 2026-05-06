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
    public class SoulListSection : UISection, IUIBindable
    {
        [SerializeField] private SoulListController[] listControllers;

        private Dictionary<SoulType, SoulListController> _soulListControllers = new();

        private SoulType _currentSoulType;

        public Action<SoulDataSO> OnItemFocused;

        public Enum BindKey => SoulUIProperty.SoulList;

        public override Selectable FirstSelectable
        {
            get
            {
                if (_soulListControllers.TryGetValue(_currentSoulType, out var soulListController))
                {
                    Debug.Log($"Soul list controller exists!");
                    return soulListController.FirstSelectable;
                }

                Debug.Log("What the fuck");
                return null;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            _soulListControllers = listControllers.ToDictionary(x => x.SoulType);
        }

        public override void InitializeView()
        {
            base.InitializeView();
            foreach (var lc in listControllers)
                lc.OnItemFocused += HandleItemFocused;
        }

        private void OnDestroy()
        {
            foreach (var lc in _soulListControllers)
                lc.Value.OnItemFocused -= HandleItemFocused;
        }

        public void Bind(object v)
        {
            if (v is not List<SoulDataSO> soulDatas) return;
            CreateSoulList(soulDatas);
        }

        public void SetCurrentSoul(SoulType soulType)
        {
            _currentSoulType = soulType;
            foreach (var slc in _soulListControllers)
                slc.Value.ToggleActive(slc.Key == soulType);
        }

        public void CreateSoulList(List<SoulDataSO> soulList)
        {
            var added = new List<Selectable>();
            var removed = new List<Selectable>();
            
            foreach (var slc in _soulListControllers)
            {
                var list = soulList.FindAll(x => x.soulType == slc.Key);
                slc.Value.CreateSoulList(list, ref added, ref removed);
            }

            foreach (var a in added)
                AddSelectable(a);
            foreach (var r in removed)
                RemoveSelectable(r);
        }

        private void HandleItemFocused(SoulDataSO holder)
        {
            OnItemFocused?.Invoke(holder);
        }

        public void SetEquipped(SoulType st, SoulDataSO data)
        {
            _soulListControllers[st].SetEquipped(data);
        }
    }
}