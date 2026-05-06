using System;
using System.Collections.Generic;
using System.Linq;
using Code.Souls.Core;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Collectibles.Inventories
{
    [Provide]
    public class SoulInventory : MonoBehaviour, IInventory<SoulDataSO>, IDependencyProvider
    {
        public event Action<SoulDataSO> OnGetSoulEvent;
        
        [SerializeField] private GameEventChannelSO skillChannel;
        [SerializeField] private SoulDataListSO soulList;

        public IEnumerable<SoulDataSO> Items => _skillActivePairs.Keys;
        private Dictionary<SoulDataSO, bool> _skillActivePairs;
        
        private void Awake()
        {
            _skillActivePairs = soulList.values.ToDictionary(data => data, _ => false);
            skillChannel.AddListener<SoulActiveEvent>(HandleSkillActive);
        }

        private void OnDestroy()
        {
            skillChannel.RemoveListener<SoulActiveEvent>(HandleSkillActive);
        }

        private void HandleSkillActive(SoulActiveEvent evt)
        {
            SoulDataSO soulData = evt.targetSoul;
            if(_skillActivePairs.ContainsKey(soulData) == false || _skillActivePairs[soulData] == evt.isActive) return;

            _skillActivePairs[soulData] = evt.isActive;
            
            if(evt.isActive)
                OnGetSoulEvent?.Invoke(soulData);
        }
        
        public bool IsActive(SoulDataSO skillData) => _skillActivePairs[skillData];

        public bool Has(SoulDataSO item) => _skillActivePairs.GetValueOrDefault(item);
    }
}