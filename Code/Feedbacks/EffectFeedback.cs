using EventSystem;
using Feedbacks.Contexts;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Feedbacks
{
    public class EffectFeedback : Feedback<EffectFeedbackContext>
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private PoolItemSO item;
        [SerializeField] private bool isRotationReset;
        [SerializeField] private bool isUseSerializeItem;

        public override void PlayFeedback()
        {
            bool canUseContext = _context != null;
            Vector3 pos = canUseContext ? _context.Position : transform.position;
            Quaternion rot = canUseContext ? _context.Rotation : transform.rotation;
            rot = isRotationReset ? Quaternion.identity : rot;
            
            PoolItemSO effectItem = canUseContext && !isUseSerializeItem ? _context.EffectPooItem : item;
            
            if (!effectItem) return;

            var evt = FeedbackEvents.EffectFeedbackEvent.Initializer(effectItem, pos, rot);
            feedbackChannel.RaiseEvent(evt);
            _context = null;
        }

        public override void StopFeedback()
        {
            
        }
    }
}