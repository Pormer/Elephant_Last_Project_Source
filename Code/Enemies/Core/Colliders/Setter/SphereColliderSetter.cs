using Code.Enemies.Core.Colliders.Data;
using UnityEngine;

namespace Code.Enemies.Core.Colliders.Setter
{
    public class SphereColliderSetter : AbstractColliderSetter
    {
        [SerializeField] SphereCollider sphereCollider;
        
        public override void SetColliderData(AbstractColliderDataSO colliderData)
        {
            if (colliderData is SphereColliderDataSO castColliderData)
            {
                sphereCollider.enabled = true;
                sphereCollider.radius = castColliderData.radius;
                sphereCollider.center = new Vector3(0, castColliderData.centerPosY, 0);
            }
        }

        public override void DisableCollider()
        {
            sphereCollider.enabled = false;
        }
    }
}