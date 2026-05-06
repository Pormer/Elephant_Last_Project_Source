using Code.Entities;
using Code.Players.Modules;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Players.States.PlayerSkillStates
{
    public abstract class PlayerCanUseSkillState : PlayerCanPreInputState
    {
        protected PlayerSkillManagement _skillManagement;
        
        public PlayerCanUseSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillManagement = entity.GetModule<PlayerSkillManagement>();
            Debug.Assert(_skillManagement != null, "state variable: skill management is null");
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnActiveSkillPressed += HandleActiveSkillPressed;
            _player.PlayerInput.OnUltimateSkillPressed += HandleUltimateSkillPressed;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnActiveSkillPressed -= HandleActiveSkillPressed;
            _player.PlayerInput.OnUltimateSkillPressed -= HandleUltimateSkillPressed;
            base.Exit();
        }
        
        protected virtual void HandleActiveSkillPressed(bool isPressed)
            => UseSkill(SkillKeyType.ACTIVE_SKILL, isPressed);
        
        protected virtual void HandleUltimateSkillPressed(bool isPressed)
            => UseSkill(SkillKeyType.ULTIMATE_SKILL, isPressed);

        protected abstract void UseSkill(SkillKeyType keyType, bool isPressed = true);
    }
}