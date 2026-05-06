using Code.Combat.Attacks;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Effects;
using Code.Skills.Core;
using DG.Tweening;
using UnityEngine;

namespace Code.Skills.DefaultSouls.DarknessSkills.TeleportSkills
{
    public class TeleportRiseObject : SkillSummon<SkillSummonContext, TeleportSkillDataSO>, IAttackable
    {
        public Transform AttackerTrm => transform;

        [SerializeField] private PlayParticleVFX effect;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private float duration;

        private Tween _releaseTween;

        public override void SetUp(SkillSummonContext context)
        {
            base.SetUp(context);

            RiseUp();
        }

        private void RiseUp()
        {
            effect.PlayVFX(transform.position, Quaternion.identity);
            damageCaster.StartCasting();
            damageCaster.CastDamage(_castedData.riseUpCasterData, _damageContext, out _);
            _releaseTween = DOVirtual.DelayedCall(duration, Release);
        }
        
        public override void Release()
        {
            base.Release();
            
            _releaseTween.Kill();
            Destroy(gameObject);
        }
    }
}