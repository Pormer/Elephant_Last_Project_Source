using Code.Entities;

namespace Code.Players.States
{
    public abstract class PlayerAirState : PlayerCanAttackState
    {
        public PlayerAirState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        
        public override void Update()
        {
            base.Update();
            
            Move();
        }
    }
}