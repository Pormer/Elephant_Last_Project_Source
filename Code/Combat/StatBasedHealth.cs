using Code.Entities.StatSystem;
using Code.Modules;
using UnityEngine;

namespace Code.Combat
{
    public class StatBasedHealth : HealthModule, IAfterInitModule
    {
        [SerializeField] protected StatSO healthStat;
        
        protected EntityStatCompo _statCompo;
        
        public void AfterInitialize()
        {
            _statCompo = _owner.GetModule<EntityStatCompo>();
            Debug.Assert(healthStat != null, "Player health component: stat component is null");

            MaxHealth = (int)_statCompo.SubscribeStat(healthStat, HandleMaxMaxHealthChange, MaxHealth);
            SetUpHealth(MaxHealth);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(healthStat, HandleMaxMaxHealthChange);
        }
        
        protected void HandleMaxMaxHealthChange(StatSO stat, float currentValue, float previousValue)
        {
            int changed = (int)(currentValue - previousValue);
            float prevHealthPercent = CurrentHealth / previousValue;

            MaxHealth = (int)currentValue;
            
            if(changed > 0)
                CurrentHealth = Mathf.RoundToInt(MaxHealth * prevHealthPercent);
            else
                CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        }
    }
}