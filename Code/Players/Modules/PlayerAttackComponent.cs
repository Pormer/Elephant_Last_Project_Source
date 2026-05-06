using System.Collections.Generic;
using System.Linq;
using Code.Combat;
using Code.Combat.Attacks;
using Code.Entities.Modules;
using Code.Entities.StatSystem;
using Code.Modules;
using Code.Players.AttackSystem;
using EventSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Players.Modules
{
    public class PlayerAttackComponent : MonoBehaviour, IModule, IAfterInitModule
    {
        public UnityEvent OnAttackEvent;
        public UnityEvent OnAttackSuccessEvent;
        public UnityEvent OnPowerAttackEvent;

        [SerializeField] private StatSO damageStatData;
        [SerializeField] private StatSO attackSpeedStatData;

        [SerializeField] private float attackSpeed = 0.7f;

        [Header("Hit Feedback")] [SerializeField]
        private GameEventChannelSO effectChannel;

        [SerializeField] private PoolItemSO powerHitEffectPoolItem;
        [SerializeField] private PoolItemSO normalHitEffectPoolItem;

        private readonly int _attackSpeedHash = Animator.StringToHash("ATTACK_SPEED");
        private readonly int _comboCountHash = Animator.StringToHash("COMBO_COUNT");
        private readonly int _comboTypeHash = Animator.StringToHash("COMBO_TYPE");

        private EntityAnimator _animator;
        private EntityRenderer _renderer;
        private EntityAnimatorTrigger _animatorTrigger;
        private EntityStatCompo _statComponent;

        private Player _owner;
        private Dictionary<AttackComboType, AttackComboHandler> _attackComboDict;

        private AttackActionDataSO _currentActionData;
        public AttackActionDataSO CurrentActionData => _currentActionData;

        public int MaxComboCount =>
            _attackComboDict.GetValueOrDefault(_currentComboType)?.AttackComboData.MaxComboCount ?? 0;

        public bool IsLastAttackSuccess { get; private set; }
        public bool CanAttack { get; private set; }

        private AttackComboType _currentComboType;
        private int _comboCount;

        public float AttackSpeed
        {
            get => attackSpeed;
            set
            {
                attackSpeed = value;
                _animator.SetParam(_attackSpeedHash, attackSpeed);
            }
        }

        public int ComboCount
        {
            get => _comboCount;
            private set
            {
                _comboCount = value;
                _animator.SetParam(_comboCountHash, _comboCount);
            }
        }

        public void Initialize(ModuleOwner owner)
        {
            _attackComboDict = new Dictionary<AttackComboType, AttackComboHandler>();
            _owner = owner as Player;

            _animator = owner.GetModule<EntityAnimator>();
            _renderer = owner.GetModule<EntityRenderer>();
            _animatorTrigger = owner.GetModule<EntityAnimatorTrigger>();
            _statComponent = owner.GetModule<EntityStatCompo>();

            Debug.Assert(_animator != null, "state variable: entity animator is null");
            Debug.Assert(_renderer != null, "state variable: entity renderer is null");
            Debug.Assert(_animatorTrigger != null, "state variable: animator trigger is null");
            Debug.Assert(_statComponent != null, "state variable: stat component is null");

            GetComponentsInChildren<AttackComboHandler>().ToList().ForEach(comboHandler =>
            {
                comboHandler.Initialize(_owner);
                _attackComboDict.TryAdd(comboHandler.AttackComboData.comboType, comboHandler);
            });

            IsLastAttackSuccess = false;
            CanAttack = true;
        }

        public void AfterInitialize()
        {
            _owner.OnHitEvent.AddListener(HandleHitEvent);
            AttackSpeed = _statComponent.SubscribeStat(attackSpeedStatData, HandleAttackSpeedChange, attackSpeed);
        }

        private void OnDestroy()
        {
            _owner.OnHitEvent.RemoveListener(HandleHitEvent);
            _statComponent.UnSubscribeStat(attackSpeedStatData, HandleAttackSpeedChange);
        }

        public void SubscribeAttackTrigger() => _animatorTrigger.OnAttackTrigger += HandleAttackTrigger;
        public void UnSubscribeAttackTrigger() => _animatorTrigger.OnAttackTrigger -= HandleAttackTrigger;

        private void HandleAttackSpeedChange(StatSO stat, float currentValue, float previousValue) =>
            AttackSpeed = currentValue;
        
        private void HandleHitEvent(int damage)
        {
            ClearComboCount();
        }

        private void HandleAttackTrigger()
        {
            if (!_attackComboDict.TryGetValue(_currentComboType, out AttackComboHandler handler) || handler == null)
            {
                Debug.LogError($"damage caster type is null: {_currentComboType}");
                return;
            }

            IsLastAttackSuccess = handler.ComboAttack(ComboCount, damageStatData, out _currentActionData,
                out HashSet<IDamageable> hits);

            ComboCount = (ComboCount + 1) % MaxComboCount;

            if (!IsLastAttackSuccess) return;

            ApplyHitEffects(hits);
            OnAttackSuccessEvent?.Invoke();

            if (_currentActionData.attackData.attackType == AttackType.Power)
                OnPowerAttackEvent?.Invoke();
        }

        private void ApplyHitEffects(HashSet<IDamageable> hits)
        {
            foreach (var damageable in hits)
            {
                var owner = damageable as ModuleOwner;
                if (owner != null)
                {
                    var targetTrm = owner.transform;
                    bool isPowerAttack = _currentActionData.attackData.attackType == AttackType.Power;
                    
                    //플레이어가 바라보는 방향에 따라 이펙트 위치와 방향 설정
                    Vector3 lookHitPoint = _currentActionData.hitPoint;
                    lookHitPoint.x *= _renderer.IsFacingRight ? 1 : -1;

                    Vector3 effectPos = _owner.transform.position + lookHitPoint;
                    effectPos.z = targetTrm.position.z;
                    
                    Quaternion effectRot = _renderer.IsFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

                    //강공격인지 확인
                    var effectItem = isPowerAttack ? powerHitEffectPoolItem : normalHitEffectPoolItem;

                    var evt = FeedbackEvents.EffectFeedbackEvent.Initializer(effectItem, effectPos, effectRot);
                    effectChannel.RaiseEvent(evt);
                }
            }
        }

        public AttackDataSO Attack(AttackComboType comboType)
        {
            if (!CanAttack) return null;

            if (_currentComboType != comboType) ClearComboCount();

            _currentComboType = comboType;
            _animator.SetParam(_comboTypeHash, (int)comboType);

            OnAttackEvent?.Invoke();
            return _attackComboDict.GetValueOrDefault(_currentComboType)?.GetCurrentAttackComboData(ComboCount);
        }

        public void ClearComboCount()
        {
            IsLastAttackSuccess = false;
            ComboCount = 0;
        }

        public void SetCanAttack(bool canAttack) => CanAttack = canAttack;
    }
}