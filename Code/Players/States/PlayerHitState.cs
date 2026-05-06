using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerHitState : PlayerState
    {
        private HealthModule _health;
        public PlayerHitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _health =  entity.GetModule<HealthModule>();
            
            Debug.Assert(_health != null, $"health module is not found: {entity.name}");
        }

        public override void Enter()
        {
            base.Enter();
            _health.CanDamageable = false;
            _movement.SetCanManualMove(false);
        }

        public override void Exit()
        {
            _health.CanDamageable = true;
            _movement.SetCanManualMove(true);
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}