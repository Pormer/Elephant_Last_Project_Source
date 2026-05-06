using System.Collections.Generic;
using System.Linq;
using Code.Modules;
using UnityEngine;

namespace Code.Entities.StatSystem
{
    public class EntityStatCompo : MonoBehaviour, IModule
    {
        [SerializeField] private StatOverrideListSO statOverrides;
        
        private Dictionary<string, StatSO> _stats;
        
        public void Initialize(ModuleOwner owner)
        {
            _stats = statOverrides.statOverrides.ToDictionary(s 
                => s.Stat.statName, s=>s.CreateStat());
        }

        public StatSO GetStat(StatSO stat)
        {
            Debug.Assert(stat != null, $"Stat: GetStat - stat can not be null");
            return _stats.GetValueOrDefault(stat.statName);
        }

        public bool TryGetStat(StatSO stat, out StatSO outStat)
        {
            Debug.Assert(stat != null, $"Stats: TryGetStat - stat cannot be null");
            outStat = _stats.GetValueOrDefault(stat.statName);
            return outStat != null;
        }

        public void SetBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat) => GetStat(stat).BaseValue;
        public void IncreaseBaseValue(StatSO stat, float value) => GetStat(stat).BaseValue += value;

        public void SetBaseStatOverrides(StatOverrideListSO statList)
        {
            foreach (StatOverride stat in statList.statOverrides)
            {
                SetBaseValue(stat.Stat, stat.Value);
            }
        }

        public void AddModifier(StatSO stat, object key, float value)
            => GetStat(stat).AddModifier(key, value);

        public void RemoveModifier(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void ClearAllStatModifier()
        {
            foreach (StatSO stat in _stats.Values)
            {
                stat.ClearModifier();
            }
        }

        public float SubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if (target == null)
            {
                Debug.Log("타겟이 스탯이 없음");
                return defaultValue;
            }
            target.OnValueChanged += handler;
            return target.Value;
        }

        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler)
        {
            StatSO target = GetStat(stat);
            if (target == null) return;
            target.OnValueChanged -= handler;
        }
    }
}