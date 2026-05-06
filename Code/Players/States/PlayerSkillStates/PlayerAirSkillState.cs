using Code.Entities;
using UnityEngine;

namespace Code.Players.States.PlayerSkillStates
{
    public class PlayerAirSkillState : PlayerSkillState
    {
        public PlayerAirSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        protected override bool CanUseSkill() => _movement.IsGrounded == false;
    }
}