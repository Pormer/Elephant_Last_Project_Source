using Code.Entities;

namespace Code.Players.States.PlayerSkillStates
{
    public class PlayerStandSkillState : PlayerSkillState
    {
        public PlayerStandSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.SetCanManualMove(false);
        }

        public override void Exit()
        {
            _movement.SetCanManualMove(true);
            base.Exit();
        }
    }
}