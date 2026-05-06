using Code.Combat;
using Code.Combat.Attacks;
using Code.Enemies.Core.Attack.AttackerData;
using Code.Entities;
using Code.Entities.StatSystem;
using EventSystem;
using UnityEngine;

namespace Code.Enemies.Core.Attack.Attacker
{
    public abstract class AbstractAttacker : MonoBehaviour
    {
        [field:SerializeField] public AttackerType AttackerType { get; private set; }
        
        public AbstactAttackerDataSO PrevAttackerData { get; private set; }
        
        protected GameEventChannelSO _feedbackChannel;
        protected DamageModule _damageModule;
        protected StatSO _damageStat;
        protected Enemy _enemy;

        public virtual void Initialize(DamageModule damageModule, Enemy enemy, StatSO damageStat, GameEventChannelSO feedbackChannel)
        {
            _enemy = enemy;
            _damageStat = damageStat;
            _damageModule = damageModule;
            _feedbackChannel = feedbackChannel;
        }

        public virtual void SetupAttacker(AbstactAttackerDataSO attackerData)
        {
            PrevAttackerData = attackerData;
        }
        
        public abstract void StartAttack();
        public abstract void EndAttack();
    }
}