using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Entities.StatSystem;
using Code.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Combat
{
    public class DamageModule : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private StatSO criticalStat;

        //크리티컬시 기본 데미지에서 추가되는 데미지(damage = default damage + (default damage * percent))
        [SerializeField] private StatSO criticalDamageStat; //퍼센트다

        private EntityStatCompo _statCompo;

        private float _criticalPercent;
        private float _criticalDamagePercent;

        public virtual void Initialize(ModuleOwner owner)
        {
            _statCompo = owner.GetModule<EntityStatCompo>();
            Debug.Assert(_statCompo != null, "state variable: stat compo is null");
        }
        
        public void AfterInitialize()
        {
            _criticalPercent = _statCompo.SubscribeStat(criticalStat, HandleCriticalPercentChanged, 0);
            _criticalDamagePercent = _statCompo.SubscribeStat(criticalDamageStat, HandleCriticalDamagePercentChanged, 0);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(criticalStat, HandleCriticalPercentChanged);
            _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamagePercentChanged);
        }

        private void HandleCriticalPercentChanged(StatSO stat, float currentValue, float previousValue) =>
            _criticalPercent = currentValue;

        private void HandleCriticalDamagePercentChanged(StatSO stat, float currentValue, float previousValue) =>
            _criticalDamagePercent = currentValue;

        public virtual DamageContext CalculateDamage(StatSO damageStat, AttackDataSO attackData, IAttackSource source,
            IAttackable attacker, out bool isCritical, bool isSelfKnockback = false, float knockbackXDir = 0f)
        {
            int damage = 0;
            isCritical = false;
            
            StatSO realStat = _statCompo.GetStat(damageStat);

            if (realStat != null)
            {
                damage = GetBaseDamage(realStat.Value, attackData);

                isCritical = Random.value < _criticalPercent;

                if (isCritical)
                    damage += (int)(damage * _criticalDamagePercent);
            }

            DamageContext context = new DamageContext(damage, source, attacker, attackData, isSelfKnockback, knockbackXDir, isCritical);

            return context;
        }

        public virtual int GetBaseDamage(float damage, AttackDataSO attackData)
        {
            return Mathf.RoundToInt(damage * attackData.damageMultiplier + attackData.damageIncrease);
        }
    }
}