using System.Collections.Generic;
using Code.Combat.Attacks;
using Code.Contexts.Explosions;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Effects;
using Code.Entities.Modules;
using Code.Extensions;
using Code.Modules;
using Code.Skills.Core;
using Code.Summons.Explosions;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Skills.DefaultSouls.DarknessSkills.BlackHoleSkills
{
    public class BlackHole : SkillSummon<SkillSummonContext, BlackHoleSkillDataSO>, IAttackable
    {
        public UnityEvent OnPullEvent;
        public UnityEvent OnPressureEvent;
        
        [SerializeField] private PlayParticleVFX[] effects;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Transform explosionTrm;
        [SerializeField] private Transform sphereTrm;

        public Transform AttackerTrm => transform;

        private readonly HashSet<ModuleOwner> _targets = new HashSet<ModuleOwner>();
        private float _skillTimer;
        private bool _isPulling;
        
        private Tween _sizeTween;
        private Tween _dissolveTween;
        private Tween _fullTween;
        private Tween _explosionTween;

        public override void SetUp(SkillSummonContext context)
        {
            base.SetUp(context);

            Init();
        }

        private void Init()
        {
            _castedData = _skill.SkillData as BlackHoleSkillDataSO;
            Debug.Assert(_castedData != null, "Black hole skill data is null");
            
            transform.localScale = Vector3.zero;
            
            //Init tweens
            _sizeTween = transform.DOScale(Vector3.one, _castedData.initSizeUpDuration).OnComplete(ActiveBlackHole);
            
            ShaderPropertyModule propertyModule = GetModule<ShaderPropertyModule>();

            _dissolveTween = DOTween.To(() => propertyModule.GetValue<float>(_castedData.dissolveProperty),
                value => propertyModule.SetValue(_castedData.dissolveProperty, value),
                1, _castedData.dissolveDuration);
        }

        private void ActiveBlackHole()
        {
            foreach (PlayParticleVFX effect in effects)
            {
                effect.PlayVFX(Vector3.zero, Quaternion.identity); // 위치, 회전 Lock 이므로 설정 X
            }

            _fullTween = DOVirtual.DelayedCall(_castedData.pullDuration, HandleFullEnd);
            _sizeTween = sphereTrm.DOScale(_castedData.sphereFinalSize, _castedData.pullDuration);
            _isPulling = true;
        }

        private void Update()
        {
            if(_isPulling == false) return;

            HandleSkillTick();
        }

        private void HandleSkillTick()
        {
            _skillTimer += Time.deltaTime;

            if (_skillTimer < _castedData.pullTerm) return;

            _skillTimer = 0;
            
            PullTargets();
            CastDamage();
        }

        private void PullTargets()
        {
            if(_targets.Count == 0) return;
            
            foreach (ModuleOwner target in _targets)
            {
                Vector3 targetPos = target.transform.position;
                Vector3 selfPos = transform.position;

                if (Vector3.Distance(targetPos, selfPos) < 0.5f) continue;
                
                EntityMovement movement = target.GetModule<EntityMovement>();
                Vector3 dir = (selfPos - targetPos).normalized;
                Vector3 velocity = dir.MultiplyElements(_castedData.pullForce);
                movement.Knockback(velocity);
            }
            
            OnPullEvent?.Invoke();
        }

        private void CastDamage()
        {
            foreach (ModuleOwner target in _targets)
            {
                damageCaster.ApplyDamageAndKnockBack(target.transform, _damageContext, out _);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TryGetTarget(other,  out ModuleOwner moduleOwner))
                _targets.Add(moduleOwner);
        }

        private void OnTriggerExit(Collider other)
        {
            if (TryGetTarget(other,  out ModuleOwner moduleOwner))
                _targets.Remove(moduleOwner);
        }

        private bool TryGetTarget(Collider other, out ModuleOwner outOwner)
        {
            bool isModuleOwner = other.TryGetComponent(out ModuleOwner moduleOwner);
            outOwner = moduleOwner;
            return other.gameObject.IsSameLayer(targetLayer) && isModuleOwner;
        }

        private void HandleFullEnd()
        {
            _isPulling = false;
            
            OnPressureEvent?.Invoke();
            
            //압축 했다가 다시 복구
            _sizeTween = sphereTrm.DOScale(_castedData.sphereFinalPressureSize, _castedData.spherePressureDuration)
                .SetEase(Ease.InQuart).SetLoops(2, LoopType.Yoyo);
            
            _explosionTween = DOVirtual.DelayedCall(_castedData.explosionDelay, () =>
            {
                Explosion();
                Release();
            });
        }

        private void Explosion()
        {
            GameObject explosionInstance = Instantiate(_castedData.blackHoleExplosionPrefab);
            DefaultExplosion explosion = explosionInstance.GetComponent<DefaultExplosion>();
            ExplosionContext context = new ExplosionContext(Owner, explosionTrm.position, Vector3.zero, _castedData.explosionData, _skill);
            explosion.SetUp(context);
        }

        public override void Release()
        {
            base.Release();
            
            _sizeTween?.Kill();
            _dissolveTween?.Kill();
            _fullTween?.Kill();
            _explosionTween?.Kill();
            Destroy(gameObject);
        }
    }
}