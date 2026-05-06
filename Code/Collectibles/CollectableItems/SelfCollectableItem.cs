using Code.Interactable;
using UnityEngine;

namespace Code.Collectibles.CollectableItems
{
    public abstract class SelfCollectableItem : CollectableItem, IInteractable
    {
        public Transform Transform => transform;

        public abstract override void CollectComplete();
        public abstract void Select(bool isSelect);

        public virtual void Interact(Transform owner)
        {
            StartCollecting(owner);
        }
    }
}