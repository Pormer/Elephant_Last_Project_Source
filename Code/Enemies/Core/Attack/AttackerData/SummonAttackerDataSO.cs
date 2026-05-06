using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Enemies.Core.Attack.AttackerData
{
    [CreateAssetMenu(fileName = "SummonAttackerDataSO", menuName = "SO/Enemy/Attacker/SummonAttackerDataSO", order = 0)]
    public class SummonAttackerDataSO : AbstactAttackerDataSO
    {
        public Vector3 summonPosition;
        public PoolItemSO summonItem;
    }
}