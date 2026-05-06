using Code.Entities;

namespace Code.Players.States
{
    public class PlayerReadyToJumpState : PlayerWaitForAnimationEndState
    {
        public PlayerReadyToJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        protected override void AnimationEnd()
        {
            _player.ChangeState("JUMP");
        }
    }
}