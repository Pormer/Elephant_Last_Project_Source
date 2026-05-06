using Code.Combat.Attacks;
using Code.Combat.KnockBacks;
using Code.Contexts.Combats;
using Code.Entities;
using Code.Entities.FSM;
using Code.Entities.Modules;
using GondrLib.Dependencies;
using Input;
using UnityEngine;

namespace Code.Players
{
    [Provide]
    public class Player : Entity, IAttackable, IAttackSource, IDependencyProvider
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateDataSO[] stateDatas;
        
        public Transform AttackerTrm { get; private set; }
        public PlayerData PlayerData { get; private set; }
        
        private EntityRenderer _renderer;
        private EntityStateMachine _stateMachine;
        
        protected override void Awake()
        {
            base.Awake();

            _stateMachine = new EntityStateMachine(this, stateDatas);
            PlayerData = GetModule<PlayerData>();
            _renderer = GetModule<EntityRenderer>();
            
            Debug.Assert(_statusCompo != null, $"Status component is null : {gameObject.name}");
            Debug.Assert(PlayerData != null, $"player data is not found : {gameObject.name}");
            Debug.Assert(_renderer != null, $"entity renderer is not found : {gameObject.name}");

            AttackerTrm = transform;
        }

        public async override void TakeDamage(DamageContext context)
        {
            if (_healthModule.CanDamageable)
            {
                await Awaitable.NextFrameAsync(destroyCancellationToken);
                _renderer.SetFacingRight(context.Attacker.AttackerTrm.position.x > AttackerTrm.position.x);
            }
            
            base.TakeDamage(context);
        }

        protected override void HandleHitEvent(int damage)
        {
            base.HandleHitEvent(damage);
            
            if (IsCurrentState("AIRBORNE"))
                return;
            ChangeState("HIT");
        }
        
        protected override void HandleDeathEvent()
        {
            ChangeState("DEATH", true);
            base.HandleDeathEvent();
        }

        protected override void HandleStopBound() { /*일단 아무것도 안함*/ }

        public override bool CheckIsAirborne()
        {
            return _stateMachine.IsCurrentState("AIRBORNE");
        }

        public override void KnockBack(KnockBackDataSO knockBackData, float xDirection)
        {
            if (knockBackData.useAirBone || CheckIsAirborne())
            {
                ChangeState("AIRBORNE");
            }
            
            base.KnockBack(knockBackData, xDirection); 
        }

        private void Start()
        {
            ChangeState("IDLE");
        }
        
        private void Update() => _stateMachine?.UpdateStateMachine();

        private void FixedUpdate() => _stateMachine?.FixedUpdateStateMachine();

        public void ChangeState(string newStateName, bool isForce = false)
            => _stateMachine?.ChangeState(newStateName, isForce);

        public bool IsCurrentState(string newStateName)
            => _stateMachine?.IsCurrentState(newStateName) ?? false;
    }
}