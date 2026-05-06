using Code.Contexts.Summons;
using Code.Entities.Modules;
using Code.Summons.Base;
using Code.Utility.Properties;
using Code.Utility.Properties.Shaders;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.ETC
{
    public class AfterImage : PoolingSummon<AfterImageContext>
    {
        public UnityEvent OnAfterImageSpawn;

        [SerializeField] private PropertyDataSO blinkData;
        
        private EntityRenderer _entityRenderer;
        private ShaderPropertyModule _propertyModule;
        private Tween _tween;

        public override void Initialize()
        {
            base.Initialize();
            
            _entityRenderer = GetModule<EntityRenderer>();
            _propertyModule = GetModule<ShaderPropertyModule>();
        }

        public override void SetUp(AfterImageContext context)
        {
            base.SetUp(context);
            
            _entityRenderer.Sprite = context.Sprite;
            _entityRenderer.SetFacingRight(context.IsFacingRight);
            
            _propertyModule.SetValue(blinkData, context.IsBlink? 1 : 0);
            
            OnAfterImageSpawn?.Invoke();
            
            _tween = DOVirtual.DelayedCall(context.Duration, () => HandleDurationEnd(context.FadeTime));
        }

        private void HandleDurationEnd(float fadeTime) => _entityRenderer.Fade(0f, fadeTime, Release);

        public override void ResetItem()
        {
            base.ResetItem();
            
            Color color = _entityRenderer.Color;
            color.a = 1f;
            _entityRenderer.Color = color;
        }

        public override void Release()
        {
            base.Release();
            
            _tween?.Kill();
        }
    }
}