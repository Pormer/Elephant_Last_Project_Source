using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerWakeUpState : PlayerWaitForAnimationEndState
    {
        HealthModule _health;
        private readonly int _GhostLayer;
        private readonly int _PlayerLayer;
        
        public PlayerWakeUpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _health = entity.GetModule<HealthModule>();
            
            Debug.Assert(_health != null, $"Health module is not found: {entity.name}");
            
            _GhostLayer = LayerMask.NameToLayer("Ghost");
            _PlayerLayer = LayerMask.NameToLayer("Player");
        }

        public override void Enter()
        {
            base.Enter();
            _health.CanDamageable = false;
            _movement.SetUseGravity(false);
            _player.gameObject.layer = _GhostLayer;
        }

        public override void Exit()
        {
            _player.gameObject.layer = _PlayerLayer;
            _health.CanDamageable = true;
            _movement.SetUseGravity(true);
            
            base.Exit();
        }

        protected override void AnimationEnd()
        {
            _player.ChangeState("IDLE");
        }
    }
}