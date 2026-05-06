using DG.Tweening;
using UnityEngine;

namespace Feedbacks
{
    public class ShakeFeedback : Feedback
    {
        [SerializeField] private Transform shakeTarget;
        [SerializeField] private float duration;
        [SerializeField] private float shakePower;
        private bool _isShaking;
        
        public override void PlayFeedback()
        {
            if (_isShaking == false)
            {
                _isShaking = true;
                shakeTarget.DOShakeScale(duration, shakePower).OnComplete(() => _isShaking = false);
            }
        }

        public override void StopFeedback()
        {
            
        }
    }
}