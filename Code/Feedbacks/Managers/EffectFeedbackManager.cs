using System;
using Code.Effects;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Feedbacks.Managers
{
    public class EffectFeedbackManager : FeedbackManager
    {
        [Inject] private PoolManagerMono _poolManager;
        
        private void Awake()
        {
            feedbackChannel.AddListener<EffectFeedbackEvent>(HandlePlayEffect);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<EffectFeedbackEvent>(HandlePlayEffect);
        }

        private void HandlePlayEffect(EffectFeedbackEvent evt)
        {
            var effect = _poolManager.Pop<PoolingEffect>(evt.effectPoolItem);
            
            if(evt.scale != Vector3.zero)
                effect.transform.localScale = evt.scale;
            effect.PlayVFX(evt.position, evt.rotation);
        }
    }
}