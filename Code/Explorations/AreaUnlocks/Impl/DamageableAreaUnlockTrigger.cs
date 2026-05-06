using Code.Combat;
using Code.Contexts;
using Code.Contexts.Combats;
using Code.Skills;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Explorations.AreaUnlocks.Impl
{
    public abstract class DamageableAreaUnlockTrigger : AreaUnlockTrigger, IDamageable
    {
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        protected HealthModule _health;

        protected override void Awake()
        {
            base.Awake();

            _health = GetModule<HealthModule>();
            Debug.Assert(_health != null, $"Health is null : {gameObject.name}");
            
            _health.OnHitEvent.AddListener(HandleHit);
            _health.OnDeadEvent.AddListener(HandleDead);
        }

        protected virtual void HandleHit(int damage)
        {
            OnHitEvent?.Invoke();
        }

        protected virtual void HandleDead()
        {
            OnDeadEvent?.Invoke();
        }

        public abstract void TakeDamage(DamageContext context);
    }
}