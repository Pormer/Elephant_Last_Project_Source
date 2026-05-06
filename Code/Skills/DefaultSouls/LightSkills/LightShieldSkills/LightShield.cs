using Code.Combat.Attacks;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Entities.Modules;
using Code.Extensions;
using Code.Skills.Core;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShieldSkills
{
    public class LightShield : SkillSummon<SkillSummonContext, LightShieldSKillDataSO>, IAttackable
    {
        public Transform AttackerTrm => transform;
        
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private LayerMask targetLayer;
        
        private Vector3 _dir;
        private Sequence _moveSeq;
        private Tween _dissolveTween;
        private ShaderPropertyModule _propertyModule;

        public override void SetUp(SkillSummonContext context)
        {
            base.SetUp(context);

            Init();
            MoveShield();
        }

        private void Init()
        {
            EntityRenderer ownerRenderer = Owner.GetModule<EntityRenderer>();
            Vector3 dir = ownerRenderer.IsFacingRight ? Vector3.right : Vector3.left;
            
            _dir = dir;
            transform.localScale = new Vector3(1, 1, 0);

            float yAngle = dir.x >= 1 ? 0 : 180;
            transform.eulerAngles = new Vector3(0, yAngle, 0);
            
            _propertyModule = GetModule<ShaderPropertyModule>();
        }

        private void MoveShield()
        {
            _moveSeq = DOTween.Sequence();
            _moveSeq.Append(transform.DOScaleZ(1, _castedData.shieldUnfoldDuration).SetEase(Ease.OutQuad));
            _moveSeq.Join(transform.DOMoveX(transform.position.x + _dir.x * _castedData.moveAmount,
                _castedData.moveDuration));
            _moveSeq.OnComplete(EndMove);
        }

        private void EndMove()
        {
            _moveSeq.Kill();
            _dissolveTween = DOTween.To(() => _propertyModule.GetValue<float>(_castedData.dissolveProperty),
                value => _propertyModule.SetValue(_castedData.dissolveProperty, value),
                1, _castedData.dissolveDuration).OnComplete(Release);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_dissolveTween != null || other.gameObject.IsSameLayer(targetLayer) == false) return;

            damageCaster.ApplyDamageAndKnockBack(other.transform, _damageContext, out _);
        }

        public override void Release()
        {
            _moveSeq?.Kill();
            _dissolveTween?.Kill();
            Destroy(gameObject);
        }
    }
}
