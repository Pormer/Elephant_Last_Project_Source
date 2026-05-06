using System.Collections;
using UnityEngine;

namespace Feedbacks
{
    public class AnimationSpeedChangeFeedback : Feedback
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float duration;
        [SerializeField] private float changeSpeed;
        
        private Coroutine _changeCoroutine;
        
        public override void PlayFeedback()
        {
            if (_changeCoroutine != null)
            {
                StopCoroutine(_changeCoroutine);
            }
            
            _changeCoroutine = StartCoroutine(ChangeSpeedCoroutine());
        }

        private IEnumerator ChangeSpeedCoroutine()
        {
            _animator.speed = changeSpeed;
            yield return new WaitForSecondsRealtime(duration);
            _animator.speed = 1;
        }

        public override void StopFeedback()
        {
            if(_changeCoroutine != null)
                StopCoroutine(_changeCoroutine);
            _animator.speed = 1;
        }
    }
}