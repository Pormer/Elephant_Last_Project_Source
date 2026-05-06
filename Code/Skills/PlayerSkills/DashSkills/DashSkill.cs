using System.Collections;
using Code.Entities.Modules;
using Code.Modules;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Skills.PlayerSkills.DashSkills
{
    public abstract class DashSkill : Skill<DashSkillDataSO>
    {
        protected EntityMovement _movement;
        protected EntityRenderer _renderer;
        protected Coroutine _dashCoroutine;

        private int _dashLayer;
        private int _defaultLayer;
        
        public override void InitializeSkill(ModuleOwner owner)
        {
            base.InitializeSkill(owner);
            
            _defaultLayer = _entity.gameObject.layer;
            _dashLayer = LayerMask.NameToLayer("DashState");
            
            _movement = owner.GetModule<EntityMovement>();
            _renderer = owner.GetModule<EntityRenderer>();
            
            Debug.Assert(_movement != null, "state variable: entity movement is null");
            Debug.Assert(_renderer != null, "state variable: entity renderer is null");
        }

        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();
            
            if(_dashCoroutine != null) StopCoroutine(_dashCoroutine);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        public override void CancelSkill()
        {
            base.CancelSkill();
            if(_dashCoroutine != null) StopCoroutine(_dashCoroutine);
            DashEnd();
        }

        protected virtual IEnumerator DashCoroutine()
        {
            SetLayer(_dashLayer);
            
            Vector3 direction = _movement.CurrentMoveDirection;
            if(direction == Vector3.zero)
                direction = _renderer.IsFacingRight ? Vector3.right : Vector3.left;
            
            _movement.SetSpeedMultiplier(_castedData.dashSpeed);
            _movement.SetMoveDirection(direction);
            
            float startTime = Time.time;
            
            while (startTime + _castedData.dashDuration > Time.time)
            {
                ApplySkillEffect();
                yield return null;
            }
            
            DashEnd();
        }

        private void DashEnd()
        {
            SetLayer(_defaultLayer);
            
            _movement.StopMovement();
            _movement.SetSpeedMultiplier();
            
            OnSkillEndEvent?.Invoke();
        }

        protected virtual void ApplySkillEffect()
        {
            
        }

        private void SetLayer(int layer)
        {
            _entity.gameObject.layer = layer;
        }
    }
}