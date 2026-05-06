using Ami.BroAudio;
using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Detectors;
using Code.Enemies.Core.Attack.AttackerData;
using Code.Entities.Modules;
using Code.Entities.StatSystem;
using Code.Utility;
using EventSystem;
using UnityEngine;

namespace Code.Enemies.Core.Attack.Attacker
{
    public class CasterAttacker : AbstractAttacker
    {
        [SerializeField] protected DamageCaster damageCaster;

        private CastAttackerDataSO _castAttackerData; 
        private EntityRenderer _renderer;
        
        public override void Initialize(DamageModule damageModule, Enemy enemy, StatSO damageStat, GameEventChannelSO feedbackChannel)
        {
            base.Initialize(damageModule, enemy, damageStat, feedbackChannel);
            _renderer = enemy.GetModule<EntityRenderer>();
        }

        public override void SetupAttacker(AbstactAttackerDataSO attackerData)
        {
            base.SetupAttacker(attackerData);
            _castAttackerData = attackerData as CastAttackerDataSO;
            
            Debug.Assert(_castAttackerData != null, "AttackerData is not CastAttackerData");
        }

        public override void StartAttack()
        {
            damageCaster.StartCasting();
            bool isImpulse = CastDamage() || _castAttackerData.isAlwaysUseImpulse;
            
            if (_castAttackerData.soundId.IsValid())
                BroAudio.Play(_castAttackerData.soundId);
            
            if (isImpulse)
                Impulse();
            
            if (_castAttackerData.effectPoolItem != null)
                StartEffect();
        }

        protected bool CastDamage()
        {
            DamageContext damageContext = _damageModule.CalculateDamage(
                _damageStat, _castAttackerData.attackData, _enemy, _enemy,  out var isCritical);
            bool isHit = damageCaster.CastDamage(_castAttackerData.detectorData, damageContext, out var damageable );

            Debug.Log($"Enemy damage : {damageContext.Damage}");
            
            
            return isHit;
        }
        
        private void Impulse()
        {
            var evt = FeedbackEvents.ImpulseFeedbackEvent.Initializer(_castAttackerData.impulseData);
            _feedbackChannel.RaiseEvent(evt);
        }
        
        private void StartEffect()
        {
            var targetTrm = _enemy.transform;

            Vector3 offset = _castAttackerData.effectPosition;
            offset.x = _renderer.IsFacingRight ? offset.x : -offset.x;
            
            Vector3 effectPos = targetTrm.localPosition + offset;
                    
            Quaternion effectRot = _renderer.IsFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            
            var evt = FeedbackEvents.EffectFeedbackEvent.Initializer(_castAttackerData.effectPoolItem, effectPos, effectRot);
            _feedbackChannel.RaiseEvent(evt);
        }
        
        public override void EndAttack()
        {
            
        }
    }
}