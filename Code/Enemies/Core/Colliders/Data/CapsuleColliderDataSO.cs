using UnityEngine;

namespace Code.Enemies.Core.Colliders.Data
{
    [CreateAssetMenu(fileName = "CapsuleDataSO", menuName = "SO/Enemy/Collider/CapsuleData", order = 0)]
    public class CapsuleColliderDataSO : SphereColliderDataSO
    {
        public float height;
    }
}