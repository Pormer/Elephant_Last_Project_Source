using UnityEngine;

namespace Code.Interactable
{
    public interface IInteractable
    {
        Transform Transform { get; }
        void Select(bool isSelect);
        void Interact(Transform owner);
    }
}