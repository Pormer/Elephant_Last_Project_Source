using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Contexts.Explosions;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Effects;
using Code.Skills.Core;
using Code.Summons.Base;
using DG.Tweening;
using UnityEngine;

namespace Code.Summons.Explosions
{
    public class Explosion<T> : Summon<T>, IAttackable where T : ExplosionContext
    {
        [SerializeField] protected DamageCaster damageCaster;
        [SerializeField] protected PlayParticleVFX particle;
        [SerializeField] protected float duration;
        
        public Transform AttackerTrm => transform;

        protected int _damage;
        protected AttackDataSO _attackData;
        protected DetectorDataSO _damageCasterData;
        protected IAttackSource _attackSource;
        private Tween _releaseTween;

        public override void SetUp(T context)
        {
            base.SetUp(context);

            Init(context);
            Explode();
            _releaseTween = DOVirtual.DelayedCall(duration, Release);
        }

        protected virtual void Init(T context)
        {
            _damage = context.ExplosionData.damage;
            _attackData = context.ExplosionData.attackData;
            _damageCasterData = context.ExplosionData.damageCasterData;
            _attackSource = context.AttackSource;
        }

        protected virtual void Explode()
        {
            particle.PlayVFX(transform.position, Quaternion.identity);
            
            DamageContext context = new DamageContext(_damage, _attackSource, this, _attackData);
            damageCaster.StartCasting();
            damageCaster?.CastDamage(_damageCasterData, context, out _);
        }

        public override void Release()
        {
            base.Release();
            
            _releaseTween?.Kill();
        }
    }
}