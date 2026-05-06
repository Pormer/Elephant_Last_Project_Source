using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Code.Combat;
using Code.Combat.KnockBacks;
using Code.Contexts.Combats;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using UnityEngine;

namespace Code.Detectors
{
    public class DamageCaster : DetectController
    {
        private HashSet<Transform> _hitObjects;
        private HashSet<IDamageable> _hits;

        protected override void Awake()
        {
            base.Awake();
            _hitObjects = new HashSet<Transform>();
            _hits = new HashSet<IDamageable>();
        }

        public virtual bool ApplyDamageAndKnockBack(Transform target, DamageContext damageContext, out IDamageable result)
        {
            if (target.TryGetComponent(out IKnockBackable knockBackable))
            {
                float xDirection;

                if (damageContext.IsSelfKnockback)
                    xDirection = damageContext.KnockbackXDir;
                else
                {
                    Vector3 attackerPos = damageContext.Attacker.AttackerTrm.position;
                    xDirection = target.position.x - attackerPos.x;
                }

                if (damageContext.AttackData.knockBackData)
                    knockBackable.KnockBack(damageContext.AttackData.knockBackData, xDirection);
            }
            
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageContext);
                result = damageable;
                return true;
            }
            
            result = null;
            return false;
        }
        
        public void StartCasting()
        {
            _hitObjects.Clear();
        }

        public bool CastDamage(DetectorDataSO detectorData, DamageContext damageContext, out HashSet<IDamageable> hits)
        {
            _hits.Clear();
            var contexts= Detect(detectorData);

            var detectContexts = contexts as DetectContext[] ?? contexts.ToArray();
            
            foreach (var context in detectContexts)
            {
                Transform target = context.Collider.transform;
                if (_hitObjects.Contains(target)) continue;
                if (ApplyDamageAndKnockBack(target, damageContext, out IDamageable damageable) == false) continue;
                _hits.Add(damageable);
                _hitObjects.Add(target);
            }

            hits = _hits;
            return _hitObjects.Any();
        }
    }
}