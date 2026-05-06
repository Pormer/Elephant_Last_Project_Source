using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Entities;
using Code.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Skills.Core
{
    public class Skill : MonoBehaviour, IAttackSource
    {
        public UnityEvent OnCoolDownCompleteEvent;
        public UnityEvent OnCoolDownStartEvent;
        //상속 받은 스킬에서 스킬 지속 시간 끝날 때 OnSkillEndEvent 호출해주세요.
        public UnityEvent OnSkillEndEvent;
        public UnityEvent OnSkillStartEvent;
        
        [field:SerializeField] public SkillDataSO SkillData { get; private set; }
        
        [Header("테스트하려고 직렬화함")]
        [SerializeField] protected Entity _entity;
        
        public float Cooldown => SkillData.cooldown;
        public float CooldownTimer => _cooldownTimer;
        public bool IsCoolDown => _cooldownTimer > 0;
        public bool IsUsing => _isUsing;

        protected DamageModule _damageModule;
        protected float _cooldownTimer;
        protected bool _isUsing;

        public virtual void InitializeSkill(ModuleOwner owner)
        {
            _entity = owner as Entity;
            _damageModule = owner.GetModule<DamageModule>();
            OnSkillEndEvent.AddListener(SkillEndHandle);
        }

        protected virtual void OnDestroy()
        {
            OnSkillEndEvent.RemoveListener(SkillEndHandle);
        }

        protected virtual void SkillEndHandle()
        {
            _isUsing = false;
            _cooldownTimer = Cooldown;
            OnCoolDownStartEvent?.Invoke();
        }

        protected virtual void Update()
        {
            if (_isUsing || _cooldownTimer <= 0) return;

            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;
                OnCoolDownCompleteEvent?.Invoke();
            }
        }

        public virtual bool UseSkill()
        {
            if (_isUsing || IsCoolDown) return false;

            _isUsing = true;

            ExecuteSkill();

            OnSkillStartEvent?.Invoke();
            return true;
        }

        protected virtual void ExecuteSkill()
        {
            //능력 발동
        }

        public virtual void CancelSkill()
        {
            _isUsing = false;
            _cooldownTimer = 0;
        }

        protected virtual DamageContext CalculateDamage(AttackDataSO attackData, IAttackable attacker, out bool isCritical)
        {
            isCritical = false;
            return _damageModule?.CalculateDamage(SkillData.majorDamageStat, attackData, this, attacker, out isCritical);
        }
#if UNITY_EDITOR
        [ContextMenu("UseSkill")]
        public virtual void TestUseSkill()
        {
            if (_isUsing || IsCoolDown) return;

            InitializeSkill(_entity);
            transform.SetParent(_entity.transform, false);

            _isUsing = true;

            ExecuteSkill();
            OnSkillStartEvent?.Invoke();
        }
#endif
    }
    
    public abstract class Skill<T> : Skill where T : SkillDataSO
    {
        protected T _castedData;

        public override void InitializeSkill(ModuleOwner owner)
        {
            base.InitializeSkill(owner);

            _castedData = SkillData as T;
            Debug.Assert(_castedData != null, $"Casted data is null : {SkillData}");
        }
    }
}