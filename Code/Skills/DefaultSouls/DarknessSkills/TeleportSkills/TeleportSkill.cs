using System;
using System.Collections.Generic;
using Code.Combat;
using Code.Contexts.Combats;
using Code.Contexts.Summons;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Entities;
using Code.Entities.Modules;
using Code.Modules;
using Code.Skills.Core;
using Code.Summons.Base;
using Code.Utility;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Skills.DefaultSouls.DarknessSkills.TeleportSkills
{
    public class TeleportSkill : Skill<TeleportSkillDataSO>
    {
        public UnityEvent OnTeleportEvent;

        [SerializeField] private CombatInfoSO combatInfo;
        [SerializeField] private TargetDetector detector;

        private HashSet<GameObject> _targets = new HashSet<GameObject>();

        private EntityRenderer _ownerRenderer;
        private EntityMovement _ownerMovement;
        private Tween _phaseTween;
        
        public override void InitializeSkill(ModuleOwner owner)
        {
            base.InitializeSkill(owner);
            
            _ownerRenderer = owner.GetModule<EntityRenderer>();
            _ownerMovement = owner.GetModule<EntityMovement>();
        }

        public override bool UseSkill()
        {
            print(combatInfo.isCombat);
            if(combatInfo.isCombat == false) return false;  
            
            return base.UseSkill();
        }

        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();
            
            if (TryGetTeleportPos(out Vector3 teleportPos))
            {
                _ownerMovement.CanManualMove = false;
                
                SpawnAfterImage(teleportPos);
                PhaseOwner(0f, () => TeleportTo(teleportPos));
            }
            else
            {
                CancelSkill();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _phaseTween?.Kill();
        }

        private bool TryGetTeleportPos(out Vector3 teleportPos)
        {
            teleportPos = Vector3.zero;
            _targets.Clear();
            
            if (detector.CheckTargetDetected(_castedData.detectorData, out _targets) == false) return false;
            
            Entity target = GetNearestTarget();
            
            if(target is null) return false;
            
            Vector3 targetPos = target.transform.position;

            return GetTeleportPosFromTarget(targetPos, out teleportPos);
        }

        private Entity GetNearestTarget()
        {
            bool TargetCondition(Entity target)
            {
                EntityMovement movement = target.GetModule<EntityMovement>();
                return movement != null && movement.IsGrounded;
            }

            return MathUtility.GetNearestTarget<Entity>(_entity.transform.position, _targets, true, TargetCondition);
        }

        private bool GetTeleportPosFromTarget(Vector3 targetPos, out Vector3 teleportPos)
        {
            // 적 기준 앞 방향
            float frontXDir = Mathf.Sign((_entity.transform.position - targetPos).x);
            Vector3 rayDir = new Vector3(frontXDir, 0f, 0f);
            
            if(CanTeleport(targetPos, rayDir, out teleportPos)) return true;
            
            // 적 기준 뒤 방향
            rayDir.x *= -1f;
            return CanTeleport(targetPos, rayDir, out teleportPos);
        }

        private bool CanTeleport(Vector3 targetPos, Vector3 dir, out Vector3 teleportPos)
        {
            teleportPos = Vector3.zero;

            if (Physics.Raycast(targetPos, dir, _castedData.rayDistance, _castedData.obstacleLayer))
                return false;

            teleportPos = targetPos + dir * _castedData.teleportOffset;
            return true;
        }

        private void SpawnAfterImage(Vector3 targetPos)
        {
            AfterImageContext context = new AfterImageContext(_entity, targetPos, Vector3.zero,
                null, false, _castedData.phaseDuration, 0f, true); // sprite, flip는 renderer모듈에서 할당
            
            _ownerRenderer.CreateAfterImage(_castedData.afterImageItem, context, true, true);
        }

        private void PhaseOwner(float endValue, Action onComplete = null)
        {
            ShaderPropertyModule propertyModule = _entity.GetModule<ShaderPropertyModule>();
            if (propertyModule == null)
            {
                onComplete?.Invoke();
                return;
            }
            
            propertyModule.SetValue(_castedData.phaseColorData, _castedData.phaseColor);
            
            _phaseTween = DOTween.To(() => propertyModule.GetValue<float>(_castedData.splitData), 
                value => propertyModule.SetValue(_castedData.splitData, value),
                endValue, _castedData.phaseDuration)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        private void TeleportTo(Vector3 teleportPos)
        {
            OnTeleportEvent?.Invoke();
            
            _ownerRenderer.SetFacingRight(_entity.transform.position.x < teleportPos.x);
            _ownerMovement.WarpTo(teleportPos);
            
            PhaseOwner(1f, () => _ownerMovement.CanManualMove = true);
            SpawnRiseObject();
        }

        private void SpawnRiseObject()
        {
            GameObject riseObjInstance = Instantiate(_castedData.riseObjectPrefab);
            TeleportRiseObject riseObject = riseObjInstance.GetComponent<TeleportRiseObject>();
            DamageContext dmgContext = CalculateDamage(_castedData.riseUpAttackData, riseObject, out _);
            SkillSummonContext summonContext = new SkillSummonContext(_entity, transform.position, Vector3.zero, this, dmgContext);
            riseObject.SetUp(summonContext);

            riseObject.OnReleaseEvent.AddListener(HandleRiseObjectReleased);
        }

        private void HandleRiseObjectReleased(Summon<SkillSummonContext> riseObj)
        {
            riseObj.OnReleaseEvent.RemoveListener(HandleRiseObjectReleased);
            OnSkillEndEvent?.Invoke();
        }
    }
}