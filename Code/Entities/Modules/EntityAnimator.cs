using Code.Modules;
using UnityEngine;

namespace Code.Entities.Modules
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour, IModule
    {
        protected Animator _animator;
        
        public void Initialize(ModuleOwner owner)
        {
            _animator = GetComponent<Animator>();
        }

        public void SetParam(int hash, float value, float dampTime) 
            => _animator.SetFloat(hash, value, dampTime, Time.deltaTime);
        
        public void SetParam(int hash, float value) => _animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => _animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => _animator.SetBool(hash, value);
        public void SetParam(int hash) => _animator.SetTrigger(hash);

        public void ForceUpdate(float deltaTime = 0) => _animator.Update(deltaTime);
        
        public void StartPlayAnimator() => _animator.StopPlayback();
        public void StopPlayAnimator() =>  _animator.StartPlayback();
        
        public void AnimatorEnable(bool isEnabled)
        {
            _animator.enabled = isEnabled;
        }
    }
}