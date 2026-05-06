using Code.Combat;
using Code.Combat.Attacks;
using Code.Combat.KnockBacks;
using Code.Contexts.Combats;
using Code.Enemies.Core.Colliders;
using Code.Enemies.Core.EnemyModules;
using Code.Entities;
using EventSystem;
using GondrLib.ObjectPool.RunTime;
using Unity.Behavior;
using UnityEngine;

namespace Code.Enemies.Core
{
    public class Enemy : Entity, IAttackable, IAttackSource, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field:SerializeField] public EntityFinderSO PlayerFinder { get; private set; }
        [field:SerializeField] public GameEventChannelSO FeedbackChannel { get; private set; }

        private EnemyDataSetter _dataSetter;
        private EnemyColliderInstaller _colliderInstaller;
        private StateChange _stateChannel;
        private Pool _pool;
        
        protected bool _isInitialized;

        public GameObject GameObject => gameObject;
        public Transform AttackerTrm => transform;
        public BattleManager BattleManager { get; private set; }
        public BehaviorGraphAgent BtAgent { get; private set; }

        public override void Initialize()
        {
            if (_isInitialized) return;
            
            base.Initialize();
            
            BtAgent = GetComponent<BehaviorGraphAgent>();
            _dataSetter = GetModule<EnemyDataSetter>();
            _colliderInstaller = GetModule<EnemyColliderInstaller>();
            
            Debug.Assert(BtAgent != null, $"{gameObject.name} : BTAgent is null");
            Debug.Assert(_dataSetter != null, $"{gameObject.name} : DataSetter is null");
        }

        protected override void AfterInitialize()
        {
            if (_isInitialized) return;
            
            _isInitialized = true;
            
            base.AfterInitialize();
        }

        public void SetUpData(EnemyDataSO enemyData)
        {
            Initialize();
            AfterInitialize();
            
            _dataSetter.SetEnemyData(enemyData);
            SetCombatData(enemyData.combatData);
            BtAgent.Graph = enemyData.btGraph;
            
            BtAgent.Init();
            _stateChannel = GetBlackboardVariable<StateChange>("StateChannel").Value;
        }
        
        protected override void HandleHitEvent(int damage)
        {
            if (IsDead) return;
            
            base.HandleHitEvent(damage);
            GameEvent evt = FeedbackEvents.DamageTextFeedbackEvent.Initializer(damage, transform.position);
            FeedbackChannel.RaiseEvent(evt);
            
            base.HandleHitEvent(damage);
        }

        protected override void HandleStopBound()
        {
            // 바운드 끝나면 일어나는 상태로 바꿔줌
            _stateChannel.SendEventMessage(EnemyStateEnum.WAKEUP);
        }

        public override bool CheckIsAirborne()
        {
            var stateVar = GetBlackboardVariable<EnemyStateEnum>("CurrentState");    
            return stateVar.Value == EnemyStateEnum.AIRBONE;
        }

        public override void TakeDamage(DamageContext context)
        {
            var stateVar = GetBlackboardVariable<EnemyStateEnum>("CurrentState");
            
            if (IsDead == false) 
                base.TakeDamage(context); // 실질적 데미지 주는 부분
            
            if (stateVar.Value == EnemyStateEnum.AIRBONE)
                return;
            
            _stateChannel.SendEventMessage(EnemyStateEnum.HIT);
        }

        public override void KnockBack(KnockBackDataSO knockBackData, float xDirection)
        {
            base.KnockBack(knockBackData, xDirection);
            
            if (knockBackData.useAirBone || _movement.IsGrounded == false)
                _stateChannel.SendEventMessage(EnemyStateEnum.AIRBONE);
            else
                _stateChannel.SendEventMessage(EnemyStateEnum.HIT);
        }

        public void ResetItem()
        {
            IsDead = false;
        }
        
        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (BtAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            return null;
        }
        
        public void SetGhostLayer(bool isGhost)
        {
            const string ghost = "Ghost";
            const string enemy = "Enemy";

            gameObject.layer = LayerMask.NameToLayer(isGhost ? ghost : enemy);
        }
        
        public void SetUpPool(Pool pool) => _pool = pool;
        public void SetBattleManager(BattleManager manager) => BattleManager = manager;
        public void DisableCollider() => _colliderInstaller.DisableColliders();
        public void DestroySelf() => _pool.Push(this);
    }
}