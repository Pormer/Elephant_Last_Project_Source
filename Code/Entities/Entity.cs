using Code.Combat;
using Code.Combat.KnockBacks;
using Code.Contexts.Combats;
using Code.Entities.Modules;
using Code.Modules;
using Code.Statuses;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public abstract class Entity : ModuleOwner, IDamageable, IKnockBackable, IStatusHandler
    {
        public EntityCombatDataSO CombatData { get; private set; }

        protected EntityMovement _movement;  
        protected HealthModule _healthModule;
        protected IManageStatus _statusCompo;
        
        public bool IsDead { get; set; }
        
        public UnityEvent<int> OnHitEvent;
        public UnityEvent<Entity> OnDeadEvent;

        public override void Initialize()
        {
            base.Initialize();
            
            _healthModule = GetModule<HealthModule>();
            _movement = GetModule<EntityMovement>();
            _statusCompo = GetModule<StatusModule>();
            
            Debug.Assert(_healthModule != null, $"Health is null : {gameObject.name}");
            Debug.Assert(_movement != null, $"Movement is null : {gameObject.name}");
            Debug.Assert(_statusCompo != null, $"StatusModule is null : {gameObject.name}");
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _healthModule.OnDeadEvent.AddListener(HandleDeathEvent);
            _healthModule.OnHitEvent.AddListener(HandleHitEvent);
            _movement.OnStopBoundEvent.AddListener(HandleStopBound);
        }

        protected virtual void OnDestroy()
        {
            _healthModule.OnDeadEvent.RemoveListener(HandleDeathEvent);
            _healthModule.OnHitEvent.RemoveListener(HandleHitEvent);
            _movement.OnStopBoundEvent.AddListener(HandleStopBound);
        }

        public Vector3 GetTargetPos(Entity targetEntity, float distance)
        {
            Vector3 targetPos = targetEntity.transform.position;
            float offset = distance + (targetEntity.CombatData.size / 2);
            targetPos.x += targetPos.x < transform.position.x ? offset : -offset;
            return targetPos;
        }
        
        public virtual void TakeDamage(DamageContext context)
        {
            _healthModule?.TakeDamage(context.Damage);
        }

        protected virtual void HandleHitEvent(int damage)
        {
            OnHitEvent?.Invoke(damage);
        }
        
        protected virtual void HandleDeathEvent()
        {
            if (IsDead) return;
            IsDead = true;
            
            OnDeadEvent?.Invoke(this);
        }

        protected abstract void HandleStopBound();
        
        public void SetCombatData(EntityCombatDataSO combatData) => CombatData = combatData;

        public virtual void KnockBack(KnockBackDataSO knockBackData, float xDirection)
        {
            if(knockBackData == null) return;
            
            Vector3 velocity = knockBackData.CalculateKnockback(xDirection);
            
            _movement.Knockback(velocity);
        }

        public abstract bool CheckIsAirborne();
        public virtual void ApplyStatus(StatusType type, float duration) => _statusCompo?.ApplyStatus(type, duration);
        public virtual void RemoveStatus(StatusType type) => _statusCompo?.RemoveStatus(type);

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (CombatData == null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, new Vector3(CombatData.size, 0.5f, 0.2f));
        }
#endif
    }
}