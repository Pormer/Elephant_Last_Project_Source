using Code.Entities;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();

            Vector2 moveDir = _player.PlayerInput.MovementKey;
            if(moveDir == Vector2.zero)
            {
                _player.ChangeState("IDLE");
                return;
            }
            
            Move();
        }
    }
}