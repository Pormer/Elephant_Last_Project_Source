using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Feedbacks.Contexts
{
    public class EffectFeedbackContext : FeedbackContext
    {
        public PoolItemSO EffectPooItem;
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        
        public EffectFeedbackContext(PoolItemSO poolItem, Vector3 position, Quaternion rotation, Vector3 scale = new())
        {
            EffectPooItem = poolItem;
            
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}