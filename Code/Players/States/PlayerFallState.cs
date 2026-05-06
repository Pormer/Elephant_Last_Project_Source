using Code.Entities;

namespace Code.Players.States
{
    public class PlayerFallState : PlayerAirState
    {
        public PlayerFallState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnDashPressed += HandleDashPressed;
        }

        public override void Update()
        {
            base.Update();

            if (!_movement.IsGrounded) return;
            
            if (NextState()) return;
            _player.ChangeState("LANDING");
        }
        
        private void HandleDashPressed()
        {
            _preInputModule.AddToBuffer("DASH", 2, false);
        }
        
        public override void Exit()
        {
            _preInputModule.ClearActionBuffer();
            
            _player.PlayerInput.OnDashPressed -= HandleDashPressed;
            
            base.Exit();
        }
    }
}