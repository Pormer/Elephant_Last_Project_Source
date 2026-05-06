using System;
using Code.Modules;
using UnityEngine;

namespace Code.Entities.Modules
{
    public class EntityAnimatorTrigger : MonoBehaviour, IModule
    {
        public event Action OnAnimationEndTrigger;
        public event Action OnDeadTrigger;
        public event Action OnAttackTrigger;
        public event Action OnAttackEndTrigger;
        public event Action OnDownSmashLandingTrigger;
        
        protected ModuleOwner _owner;
        
        public virtual void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }

        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        private void DeadTrigger() => OnDeadTrigger?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();
        private void AttackEndTrigger() => OnAttackEndTrigger?.Invoke();
        private void OnDownSmashLanding() => OnDownSmashLandingTrigger?.Invoke();
    }
}