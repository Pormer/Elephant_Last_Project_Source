using Code.Enemies.Core.Colliders.Data;
using UnityEngine;

namespace Code.Enemies.Core.Colliders.Setter
{
    public abstract class AbstractColliderSetter : MonoBehaviour
    {
        [field:SerializeField] public ColliderType ColliderType { get; private set; }

        public abstract void SetColliderData(AbstractColliderDataSO colliderData);
        public abstract void DisableCollider();
    }
}