using Code.Combat.Attacks;
using Code.Contexts.Combats;
using Code.Detectors;
using Code.Entities.Modules;
using Code.Modules;
using Code.Skills.Core;
using Code.Utility.Properties.Shaders;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Skills.DefaultSouls
{
    public class StrongPaunchSkill : Skill<StrongPaunchSkillDataSO>
    {
        public UnityEvent OnAttackEvent;
        
        [SerializeField] private DamageCaster damageCaster;

        private EntityAnimatorTrigger _ownerAnimTrigger;
        private ShaderPropertyModule _shaderPropertyModule;
        private EntityRenderer _ownerRenderer;
        private EntityMovement _ownerMovement;
        private Color _originColor;
        private IAttackable _attackable;

        public override void InitializeSkill(ModuleOwner owner)
        {
            base.InitializeSkill(owner);
            
            _ownerAnimTrigger = owner.GetModule<EntityAnimatorTrigger>();
            _shaderPropertyModule = owner.GetModule<ShaderPropertyModule>();
            _ownerRenderer = owner.GetModule<EntityRenderer>();
            _ownerMovement = owner.GetModule<EntityMovement>();
            
            _attackable = owner as IAttackable;
            Debug.Assert(_attackable != null, $"Owner is not attackable : {owner}");
        }

        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();

            _ownerMovement.CanManualMove = false;
            damageCaster.StartCasting();
            
            _originColor = _shaderPropertyModule.GetValue<Color>(_castedData.emissionProperty);
            _shaderPropertyModule.SetValue(_castedData.emissionProperty, _castedData.emissionColor);
            
            _ownerAnimTrigger.OnAttackTrigger += HandleAttackTrigger;
            _ownerAnimTrigger.OnAnimationEndTrigger += HandleAnimationEnd;
        }

        private void HandleAttackTrigger()
        {
            int xDir = _ownerRenderer.IsFacingRight ? 1 : -1;
            Vector3 force = _castedData.attackData.startAttackForce;
            force.x *= xDir;
            
            _ownerMovement.StopMovement();
            _ownerMovement.AddForce(force);
            
            DamageContext context = CalculateDamage(_castedData.attackData, _attackable, out _);
            damageCaster.CastDamage(_castedData.damageCasterData, context, out _);
            
            OnAttackEvent?.Invoke();
        }

        private void HandleAnimationEnd()
        {
            _shaderPropertyModule.SetValue(_castedData.emissionProperty, _originColor);
            
            _ownerAnimTrigger.OnAttackTrigger -= HandleAttackTrigger;
            _ownerAnimTrigger.OnAnimationEndTrigger -= HandleAnimationEnd;
            
            _ownerMovement.CanManualMove = true;
            
            OnSkillEndEvent?.Invoke();
        }
    }
}