using Code.Entities;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerJumpState : PlayerAirState
    {
        private const float waitForTime = 0.1f;
        private float _startTime;
        
        public PlayerJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _startTime = Time.time;
            _movement.Jump();
            _movement.SetSpeedMultiplier(0.9f);
        }

        public override void Update()
        {
            base.Update();

            if (_movement.VerticalVelocity < 0)
            {
                _player.ChangeState("FALL");
            }
            
            if (Time.time - _startTime < waitForTime) return;

            NextState();
            _preInputModule.ClearActionBuffer();
        }

        public override void Exit()
        {
            _movement.SetSpeedMultiplier();
            
            base.Exit();
        }
    }
}