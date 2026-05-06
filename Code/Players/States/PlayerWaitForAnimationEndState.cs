using Code.Entities;

namespace Code.Players.States
{
    public abstract class PlayerWaitForAnimationEndState : PlayerState
    {
        protected PlayerWaitForAnimationEndState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopMovement();
            _movement.SetCanManualMove(false);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
            {
                AnimationEnd();
                return;
            }
        }
        
        public override void Exit()
        {
            _movement.SetCanManualMove(true);
            base.Exit();
        }

        protected abstract void AnimationEnd();
    }
}