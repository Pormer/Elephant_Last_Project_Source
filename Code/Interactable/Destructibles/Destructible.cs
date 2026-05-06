using Code.Combat;
using Code.Contexts.Combats;
using Code.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Interactable.Destructibles
{
    public class Destructible : ModuleOwner, IDamageable
    {
        public bool IsDead { get; set; }
        
        public UnityEvent<int> OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        private HealthModule _healthModule;

        public override void Initialize()
        {
            base.Initialize();
            
            _healthModule = GetModule<HealthModule>();
            
            Debug.Assert(_healthModule != null, $"Health is null : {gameObject.name}");
        }
    
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _healthModule.OnDeadEvent.AddListener(HandleDeathEvent);
            _healthModule.OnHitEvent.AddListener(HandleHitEvent);
        }

        private void HandleHitEvent(int damage)
        {
            OnHitEvent?.Invoke(damage);
        }

        private void HandleDeathEvent()
        {
            if (IsDead) return;
            IsDead = true;
            
            OnDeadEvent?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _healthModule.OnDeadEvent.RemoveListener(HandleDeathEvent);
            _healthModule.OnHitEvent.RemoveListener(HandleHitEvent);
        }

        public void TakeDamage(DamageContext context) => _healthModule?.TakeDamage(context.Damage);
    }
}