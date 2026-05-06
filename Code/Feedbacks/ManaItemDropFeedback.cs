using Code.ManaSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Feedbacks
{
    public class ManaItemDropFeedback : Feedback
    {
        [SerializeField] private Vector3 startPosOffset;
        [SerializeField] private PoolItemSO manaItem;
        [SerializeField] private float forcePower;
        [SerializeField] private float dropPercent = 0.7f;
        
        public override void PlayFeedback()
        {
            if (dropPercent < Random.value) return;
            
            var item = PoolManagerMono.Instance.Pop<AddManaCollectItem>(manaItem);
            item.transform.position = transform.position + startPosOffset;

            Vector3 force = Random.insideUnitSphere* forcePower;
            force.y = Mathf.Abs(force.y);
            item.AddForce(force);
        }

        public override void StopFeedback()
        { 
        }
    }
}