using Code.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class HealthModule : MonoBehaviour, IModule
    {
        public bool CanDamageable { get; set; } = true;

        public UnityEvent<int> OnHealEvent;
        public UnityEvent<int> OnHitEvent;
        public UnityEvent<int, int> OnHealthChangeEvent;
        public UnityEvent OnDeadEvent;

        [field: SerializeField] public int MaxHealth { get; protected set; } = 100;

        private int _currentHealth;
        public int CurrentHealth
        {
            get => _currentHealth;
            protected set
            {
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnHealthChangeEvent?.Invoke(_currentHealth, MaxHealth);
            }
        }
        
        protected ModuleOwner _owner;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            SetUpHealth(MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            if(CanDamageable == false) return;

            CurrentHealth -= damage;
            OnHitEvent?.Invoke(damage);
            
            if(CurrentHealth <= 0)
                OnDeadEvent?.Invoke();
        }

        public void Heal(int heal)
        {
            CurrentHealth += heal; 
            OnHealEvent?.Invoke(heal);
        }

        public void SetUpHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}