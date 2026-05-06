using Code.Entities;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopMovement();
            
            CheckMoveState();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            CheckMoveState();
        }
        
        private void CheckMoveState()
        {
            if (_player.PlayerInput.MovementKey != Vector2.zero)
                _player.ChangeState("MOVE");
        }
    }
}