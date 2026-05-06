using Code.Entities.Modules;

namespace Code.Entities.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;
        protected int _animationHash;
        protected EntityAnimator _entityAnimator;
        protected EntityAnimatorTrigger _animatorTrigger;
        protected bool _isTriggerCall;

        protected EntityState(Entity entity, int animationHash)
        {
            _entity = entity;
            _animationHash = animationHash;
            _entityAnimator = entity.GetModule<EntityAnimator>();
            _animatorTrigger = entity.GetModule<EntityAnimatorTrigger>();
        }

        public virtual void Enter()
        {
            _entityAnimator.SetParam(_animationHash, true);
            _isTriggerCall = false;
            _animatorTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }

        public virtual void Update(){ }
        
        public virtual void FixedUpdate(){ }

        public virtual void Exit()
        {
            _animatorTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
            _entityAnimator.SetParam(_animationHash, false);
            _entityAnimator.ForceUpdate();
        }

        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}