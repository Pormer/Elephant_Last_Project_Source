using System;
using System.Collections.Generic;
using Code.Entities.StatSystem;
using Code.Modules;
using Code.Souls.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Code.ManaSystem
{
    public class ManaModule : MonoBehaviour, IModule, IAfterInitModule
    {
        public UnityEvent OnCollectMana;
        
        [SerializeField] private StatSO maxManaStatData;
        [SerializeField] private StatSO additionalManaGainPercentStatData;
        [SerializeField] private int startManaValue;

        public event Action<SoulType, int, int> OnManaValueChanged; //타입과 현재, 최대마나
        
        private Dictionary<SoulType, int> _soulManaDataDict;

        private int _maxManaValue;
        private float _additionalManaPercent;
        private EntityStatCompo _statCompo;

        public void Initialize(ModuleOwner owner)
        {
            _soulManaDataDict = new Dictionary<SoulType, int>();
            _statCompo = owner.GetModule<EntityStatCompo>();

            Debug.Assert(_statCompo != null, $"stat compo not fount: {owner.name}");
        }

        public void AfterInitialize()
        {
            _maxManaValue = (int)_statCompo.SubscribeStat(maxManaStatData, HandleMaxManaValueChanged, 50);
            _additionalManaPercent = _statCompo.SubscribeStat(additionalManaGainPercentStatData,
                HandleAdditionalManaPercentChanged, 50);

            for (int i = 0; i < (int)SoulType.END; i++)
            {
                var type = (SoulType)i;
                _soulManaDataDict.Add(type, startManaValue);
                SetSoulManaValue(type, startManaValue);
            }
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(maxManaStatData, HandleMaxManaValueChanged);
            _statCompo.UnSubscribeStat(additionalManaGainPercentStatData, HandleAdditionalManaPercentChanged);
        }

        private void HandleMaxManaValueChanged(StatSO stat, float currentValue, float previousValue)
        {
            _maxManaValue = (int)currentValue;
            
            for (int i = 0; i < (int)SoulType.END; i++)
            {
                SoulType type = (SoulType)i;
                SetSoulManaValue(type, GetSoulManaValue(type));
            }
        }

        private void HandleAdditionalManaPercentChanged(StatSO stat, float currentValue, float previousValue)
        {
            _additionalManaPercent = currentValue;
        }

        public void AddManaValue(SoulType type, int value)
        {
            int addValue = GetFinalAddedValue(value);
            int newValue = GetSoulManaValue(type) + addValue;

            SetSoulManaValue(type, newValue);
            
            OnCollectMana?.Invoke();
        }

        public void UsedManaValue(SoulType type, int value) 
            => SetSoulManaValue(type, GetSoulManaValue(type) - value);

        public int GetSoulManaValue(SoulType type) => _soulManaDataDict.GetValueOrDefault(type);

        public void SetSoulManaValue(SoulType type, int value)
        {
            if (!_soulManaDataDict.ContainsKey(type)) return;
            
            _soulManaDataDict[type] = Mathf.Clamp(value, 0, _maxManaValue);
            OnManaValueChanged?.Invoke(type, value, _maxManaValue);
        }

        //추가될 마나의 추가적으로 부여되는 마나를 계산한 값 반환
        private int GetFinalAddedValue(int value) => Mathf.RoundToInt(value * _additionalManaPercent);
    }
}