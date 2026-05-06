using Code.Utility.Properties;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using Feedbacks.Contexts;
using UnityEngine;

namespace Feedbacks
{
    public class BlackoutFeedback : Feedback<FeedbackContext>
    {
        [SerializeField] private ShaderPropertyManagerSO propertyManager;
        [SerializeField] private PropertyDataSO isDeadProperty;
        [SerializeField] private PropertyDataSO deadColorProperty;
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
            SetDead(1);
            _blinkTween = DOTween.To(() => Color.white, SetColor, new Color(0.55f,0.55f,0.55f), duration);
        }
        
        private void SetDead(int value) => propertyManager.SetProperty(_mat, isDeadProperty, value);
        private void SetColor(Color value) => propertyManager.SetProperty(_mat, deadColorProperty, value);

        public override void StopFeedback()
        {
            _blinkTween?.Kill();
        }
    }
}
