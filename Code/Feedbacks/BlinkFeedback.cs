using Code.Utility.Properties;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using Feedbacks.Contexts;
using UnityEngine;

namespace Feedbacks
{
    public class BlinkFeedback : Feedback<FeedbackContext>
    {
        [SerializeField] private ShaderPropertyManagerSO propertyManager;
        [SerializeField] private PropertyDataSO blinkProperty;
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private float duration;
        
        private Material _mat;
        private Tween _blinkTween;

        private void Awake()
        {
            _mat = targetRenderer.material;
        }

        public override void PlayFeedback()
        {
            _blinkTween?.Kill();
            SetHit(true);
            _blinkTween = DOVirtual.DelayedCall(duration, () => SetHit(false));
        }
        
        private void SetHit(bool value) => propertyManager.SetProperty(_mat, blinkProperty, value ? 1 : 0);
        
        public override void StopFeedback()
        {
            _blinkTween?.Kill();
        }
    }
}