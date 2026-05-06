using System;
using Code.Combat.Attacks;
using Code.Entities;
using Code.Players.AttackSystem;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerAttackState : PlayerCanAttackState
    {
        private MovementAttackDataSO _currentAttackData;
        private bool _isJumpPressed;

        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            AttackComboType comboType;
            if (_movement.IsGrounded)
                comboType = _attackCompo.IsLastAttackSuccess ? AttackComboType.GROUND_HIT : AttackComboType.GROUND_MISS;
            else
                comboType = _attackCompo.IsLastAttackSuccess ? AttackComboType.FLY_HIT : AttackComboType.FLY_MISS;

            if (_movement.IsGrounded) _attackCompo.SetCanAttack(true);

            if (!_attackCompo.CanAttack)
            {
                _player.ChangeState("IDLE");
                return;
            }

            _currentAttackData = _attackCompo.Attack(comboType) as MovementAttackDataSO;
            _attackCompo.OnAttackSuccessEvent.AddListener(HandleAttackSuccess);

            base.Enter();

            _isJumpPressed = false;

            _player.PlayerInput.OnDashPressed += HandleDashPressed;
            _player.PlayerInput.OnJumpPressed += HandleJumpPressed;
            _attackCompo.SubscribeAttackTrigger();

            _movement.SetCanManualMove(false);

            SetFacingRight();

            if (!_currentAttackData) return;
            ApplyAttackMovement(_currentAttackData.startAttackForce);
        }

        private void ApplyAttackMovement(Vector3 force)
        {
            float moveDirection = _renderer.IsFacingRight ? 1f : -1f;

            var movementVelocity = force;
            movementVelocity.x *= moveDirection;
            movementVelocity.z *= moveDirection;

            _movement.AddForce(movementVelocity);
        }

        private void SetFacingRight()
        {
            Vector2 moveDir = _player.PlayerInput.MovementKey;
            if (Mathf.Abs(moveDir.x) > 0)
                _renderer.SetFacingRight(moveDir.x > 0);
        }

        private void HandleDashPressed()
        {
            if (_movement.IsGrounded)
            {
                // _player.ChangeState("DASH");
                _preInputModule.AddToBuffer("DASH", 0, false);
            }
        }

        private void HandleJumpPressed() => _isJumpPressed = true;

        private void HandleAttackSuccess()
        {
            _movement.StopMovement();
            ApplyAttackMovement(_currentAttackData.successAttackForce);
        }

        public override void Update()
        {
            base.Update();

            if (_isAttackEndTrigger)
                AttackEnd();
            if (_isTriggerCall)
                AnimationEnd();
        }

        private void AttackEnd()
        {
            _isAttackEndTrigger = false;

            CheckJumpPressed();

            _movement.SetUseGravity(true);
            CheckCanNextAttack();
        }

        private void AnimationEnd()
        {
            _isTriggerCall = false;
            CheckJumpPressed();

            if (CheckCanNextAttack()) return;

            _attackCompo.ClearComboCount();
            _player.ChangeState("IDLE");
        }

        private void CheckJumpPressed()
        {
            if (_movement.IsGrounded && _isJumpPressed)
            {
                _preInputModule.ClearActionBuffer();
                _preInputModule.AddToBuffer("JUMP", 0, false);
            }
        }

        private bool CheckCanNextAttack()
        {
            if (!_attackCompo.IsLastAttackSuccess && !_movement.IsGrounded)
            {
                _attackCompo.SetCanAttack(false);
                _preInputModule.ClearActionBuffer();
                return false;
            }

            //콤보가 가능하다면 선입력 검사하고 리턴
            if (!_attackCompo.CurrentActionData.canContinueAttack
                && _preInputModule.NextChangeStateName.Equals("ATTACK", StringComparison.Ordinal))
            {
                _preInputModule.ClearActionBuffer();
                return false;
            }
            
            if (NextState())
            {
                if (!_preInputModule.LastChangeStateName.Equals("ATTACK", StringComparison.Ordinal)) _attackCompo.ClearComboCount();
                return true;
            }

            _preInputModule.ClearActionBuffer();
            return false;
        }

        public override void Exit()
        {
            _movement.StopMovement();
            _movement.SetUseGravity(true);
            _movement.SetCanManualMove(true);

            _attackCompo.UnSubscribeAttackTrigger();

            _player.PlayerInput.OnDashPressed -= HandleDashPressed;
            _player.PlayerInput.OnJumpPressed -= HandleJumpPressed;

            _attackCompo.OnAttackSuccessEvent.RemoveListener(HandleAttackSuccess);

            base.Exit();
        }
    }
}