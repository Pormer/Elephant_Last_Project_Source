using System.Collections.Generic;
using Code.Combat;
using Code.Enemies.Core.Attack.Attacker;
using Code.Enemies.Core.Attack.AttackerData;
using Code.Entities.Modules;
using Code.Entities.StatSystem;
using Code.Modules;
using EventSystem;
using UnityEngine;

namespace Code.Enemies.Core.Attack
{
    public class EnemyAttackController : MonoBehaviour, IModule, IAfterInitModule, IEnemyDataSettable
    {
        [SerializeField] private StatSO damageStat;
        [SerializeField] private GameEventChannelSO feedbackChannel;
        
        private Dictionary<AttackerType, AbstractAttacker> _attackerDict;
        private List<AbstactAttackerDataSO> _attackerDataList;
        
        private EntityAnimatorTrigger _trigger;
        private AbstractAttacker _attacker;
        private DamageModule _damageModule;
        private Enemy _enemy;

        private int _attackIndex;
        
        public void Initialize(ModuleOwner owner)
        {
            _enemy = owner as Enemy;
            _trigger = owner.GetModule<EntityAnimatorTrigger>();
            _damageModule = owner.GetModule<DamageModule>();
            _attackerDict = new Dictionary<AttackerType, AbstractAttacker>();

            foreach (AbstractAttacker attacker in GetComponentsInChildren<AbstractAttacker>())
            {
                attacker.Initialize(_damageModule, _enemy, damageStat, feedbackChannel);       
                _attackerDict.Add(attacker.AttackerType, attacker);
            }
        }
        
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            _attackerDataList = enemyData.attackerDataList;
        }
        
        public void AfterInitialize()
        {
            _trigger.OnAttackTrigger += StartAttack;
        }

        private void OnDestroy()
        {
            _trigger.OnAttackTrigger -= StartAttack;
        }

        public void StartAttack()
        {
            AbstactAttackerDataSO attackerData = _attackerDataList[_attackIndex];
            
            // 어태커를 찾아오기
            if (_attackerDict.TryGetValue(attackerData.AttackerType, out AbstractAttacker attacker) == false)
            {
                Debug.LogWarning("Do not have attacker for type");
                return;
            }
            
            // 만약에 이전 공격과 같은 형태의 공격이면 세팅 안하게 
            if (attacker.PrevAttackerData != attackerData)
                attacker.SetupAttacker(attackerData);
            
            // 실질적 공격
            attacker.StartAttack();
            _attacker = attacker;
        }

        // 지속되는 공격 방식일 경우에 사용하기 위해서 만듬
        public void EndAttack()
        {
            if (_attacker == null)
                return;
            
            _attacker.EndAttack();
            _attacker = null;
        }

        public void SelectAttackData(int idx)
        {
            _attackIndex = idx;
        }
    }
}