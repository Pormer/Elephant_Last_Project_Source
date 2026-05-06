using Code.Enemies.Core.Colliders.Data;
using UnityEngine;

namespace Code.Enemies.Core.Colliders.Setter
{
    public class CapsuleColliderSetter : AbstractColliderSetter
    {
        [SerializeField] CapsuleCollider capsuleCollider;
        
        public override void SetColliderData(AbstractColliderDataSO colliderData)
        {
            if (colliderData is CapsuleColliderDataSO castColliderData)
            {
                capsuleCollider.enabled = true;
                
                capsuleCollider.height = castColliderData.height;
                capsuleCollider.radius = castColliderData.radius;
                capsuleCollider.center = new Vector3(0, castColliderData.centerPosY, 0);
            }
        }
        
        public override void DisableCollider()
        {
            capsuleCollider.enabled = false;
        }
    }
}       