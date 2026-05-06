using Code.Combat.Attacks;
using Feedbacks.Contexts;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Enemies.Core.Attack.AttackerData
{
    public abstract class AbstactAttackerDataSO : ScriptableObject
    {
        [field:SerializeField] public AttackerType AttackerType { get; private set; }
        public AttackDataSO attackData;
        
        [Header("Effect")]
        public PoolItemSO effectPoolItem;
        public Vector3 effectPosition;
        
        [Header("Impulse")]
        public ImpulseFeedbackDataSO impulseData;
    }
}