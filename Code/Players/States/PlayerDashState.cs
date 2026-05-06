using Code.Entities;
using Code.Players.Modules;
using Code.Skills.PlayerSkills.DashSkills;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerDashState : PlayerCanPreInputState
    {
        private DashSkillSelector _dashSelector;
        private DashSkill _targetDashSkill;

        private bool isJumpPressed;
        private bool isAttackEndTrigger;

        public PlayerDashState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _dashSelector = entity.GetModule<DashSkillSelector>();
            Debug.Assert(_dashSelector != null, "state variable: dash selector is null");
        }

        public override void Enter()
        {
            base.Enter();

            _targetDashSkill = _dashSelector.CurrentActiveDashSkill as DashSkill;
            Debug.Assert(_targetDashSkill != null, "state variable: target dash skill is null");

            isJumpPressed = false;
            isAttackEndTrigger = false;
            
            _player.PlayerInput.OnJumpPressed += HandleJumpPressed;
            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
            _animatorTrigger.OnAttackEndTrigger += HandleAttackEndTrigger;

            Move();
            
            if (!_targetDashSkill.UseSkill())
            {
                _player.ChangeState("IDLE");
            }
        }

        private void HandleAttackPressed()
        {
            _preInputModule.ClearActionBuffer();
            _preInputModule.AddToBuffer("ATTACK", 1);
        }

        private void HandleJumpPressed()
        {
            if(isAttackEndTrigger && !isJumpPressed)
            {
                ChangeJumpState();
                return;
            }
            
            isJumpPressed = true;
        }

        private void HandleAttackEndTrigger() => isAttackEndTrigger = true;

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
                HandleDashComplete();
            if (isAttackEndTrigger)
                CheckAttackEnd();
        }

        private void CheckAttackEnd()
        {
            isAttackEndTrigger = false;
            if (isJumpPressed)
            {
                ChangeJumpState();
            }
        }
        
        private void HandleDashComplete()
        {
            if (NextState()) return;
            
            _player.ChangeState("IDLE");
        }
        
        private void ChangeJumpState()
        {
            if(_movement.IsGrounded)
                _player.ChangeState("JUMP");
        }

        public override void Exit()
        {
            _targetDashSkill.CancelSkill();
            
            _player.PlayerInput.OnJumpPressed -= HandleJumpPressed;
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            _animatorTrigger.OnAttackEndTrigger -= HandleAttackEndTrigger;

            base.Exit();
        }
    }
}