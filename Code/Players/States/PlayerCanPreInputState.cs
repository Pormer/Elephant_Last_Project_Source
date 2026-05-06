using Code.Entities;
using Code.Players.Modules;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerCanPreInputState : PlayerCanMoveState
    {
        protected PlayerPreInputModule _preInputModule;
        protected bool _isAttackEndTrigger;
        
        protected PlayerCanPreInputState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _preInputModule = entity.GetModule<PlayerPreInputModule>();
            Debug.Assert(_preInputModule != null, $"Pre-Input module is not found: {entity.name}");
        }

        public override void Enter()
        {
            base.Enter();
            _isAttackEndTrigger = false;
            _animatorTrigger.OnAttackEndTrigger += AttackEndTrigger;
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackEndTrigger -= AttackEndTrigger;
            
            base.Exit();
        }
        
        protected bool NextState()
        {
            if (_preInputModule.CheckNextAction(out string stateName))
            {
                _player.ChangeState(stateName, true);
                return true;
            }
            
            return false;
        }

        private void AttackEndTrigger() => _isAttackEndTrigger = true;
    }
}