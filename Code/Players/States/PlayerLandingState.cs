using Code.Entities;

namespace Code.Players.States
{
    public class PlayerLandingState : PlayerWaitForAnimationEndState
    {
        public PlayerLandingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.OnLandedEvent.Invoke();
        }

        protected override void AnimationEnd()
        {
            _player.ChangeState("IDLE");
        }
    }
}