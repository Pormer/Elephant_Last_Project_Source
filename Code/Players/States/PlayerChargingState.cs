using Code.Entities;
using Code.Players.States.PlayerSkillStates;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerChargingState : PlayerCanUseSkillState
    {
        private readonly int _chargingEndTrigger = Animator.StringToHash("CHARGING_END");
        
        public PlayerChargingState(Entity entity, int animationHash) : base(entity, animationHash)
        { }

        public override void Update()
        {
            base.Update();
            
            if(_isTriggerCall) 
                _player.ChangeState("IDLE");
        }

        protected override void UseSkill(SkillKeyType keyType, bool isPressed = true)
        {
            if (isPressed) return;

            var skill = _skillManagement.GetSkill(keyType);
            if (skill is IChargeable { IsCharging: true } chargeable)
            {
                _skillManagement.TryUseSkill(skill);
                chargeable.ReleaseCharging();
                _entityAnimator.SetParam(_chargingEndTrigger);
            }
        }
    }
}