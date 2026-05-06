using Code.Entities;
using Code.Interactable;
using Code.Players.Modules;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerGroundedState : PlayerCanAttackState
    {
        private DashSkillSelector _dashSelector;
        private Interactor _interactor;
        
        public PlayerGroundedState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _dashSelector = entity.GetModule<DashSkillSelector>();
            _interactor = entity.GetModule<Interactor>();
            Debug.Assert(_dashSelector != null, "state variable: dash selector is null");
            Debug.Assert(_interactor != null, "state variable: interactor is null");
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnJumpPressed += HandleJumpPressed;
            _player.PlayerInput.OnDashPressed += HandleDashPressed;
            _player.PlayerInput.OnInteractPressed += HandleInteractPressed;
        }

        public override void Update()
        {
            base.Update();

            if (!_movement.IsGrounded)
            {
                _player.ChangeState("FALL");
            }
        }

        private void HandleDashPressed()
        {
            var skill = _dashSelector.CurrentActiveDashSkill;
            if (skill == null || skill.IsCoolDown)
            {
                //스킬 없음 Feedback보내기
                return;
            }
            
            if (_movement.IsGrounded)
                _player.ChangeState("DASH");
        }
        
        private void HandleJumpPressed()
        {
            if (_movement.IsGrounded)
            {
                _player.ChangeState("JUMP");
            }
        }

        private void HandleInteractPressed()
        {
            if (_movement.IsGrounded && _interactor.GetCurrentInteractableItem() != null)
            {
                _player.ChangeState("INTERACT");
            }
        }
        
        protected override void HandleAttackPressed()
        {
            /*_preInputModule.ClearActionBuffer();
            base.HandleAttackPressed();
            
            if (NextState()) return;
            _player.ChangeState("IDLE");*/
            _player.ChangeState("ATTACK");
        }

        public override void Exit()
        {
            _player.PlayerInput.OnInteractPressed -= HandleInteractPressed;
            _player.PlayerInput.OnJumpPressed -= HandleJumpPressed;
            _player.PlayerInput.OnDashPressed -= HandleDashPressed;
            base.Exit();
        }
    }
}