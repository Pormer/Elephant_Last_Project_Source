using UnityEngine;

namespace Code.Enemies.Core.Colliders.Data
{
    [CreateAssetMenu(fileName = "SphereDataSO", menuName = "SO/Enemy/Collider/SphereData", order = 0)]
    public class SphereColliderDataSO : AbstractColliderDataSO
    {
        public float centerPosY;
        public float radius;
    }
}