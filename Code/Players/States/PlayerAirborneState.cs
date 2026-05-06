using Code.Entities;

namespace Code.Players.States
{
    public class PlayerAirborneState : PlayerState
    {
        private bool _isBoundEnd;
        
        public PlayerAirborneState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _isBoundEnd = false;
            
            _movement.SetCanManualMove(false);
            _movement.OnStopBoundEvent.AddListener(HandleStopBound);
        }

        public override void Update()
        {
            base.Update();

            if (!_isBoundEnd) return;
            
            _isBoundEnd = false;
            _player.ChangeState("WAKEUP");
        }

        private void HandleStopBound() => _isBoundEnd = true;

        public override void Exit()
        {
            _movement.SetCanManualMove(true);
            _movement.OnStopBoundEvent.RemoveListener(HandleStopBound);
            
            base.Exit();
        }
    }
}