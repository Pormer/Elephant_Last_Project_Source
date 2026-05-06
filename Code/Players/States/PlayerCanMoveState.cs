using Code.Entities;
using Code.Entities.Modules;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerCanMoveState : PlayerState
    {
        protected EntityRenderer _renderer;
        
        public PlayerCanMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _renderer = entity.GetModule<EntityRenderer>();
            Debug.Assert(_renderer != null, "state variable: renderer is null");
        }

        protected void Move()
        {
            Vector2 moveDir = _player.PlayerInput.MovementKey;
            
            _movement.SetMoveDirection(moveDir);
            
            if(Mathf.Abs(moveDir.x) > 0)
                _renderer.SetFacingRight(moveDir.x > 0);
        }
    }
}