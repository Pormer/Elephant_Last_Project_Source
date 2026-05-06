using System;
using System.Collections.Generic;
using System.Linq;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using Code.Modules;
using EventSystem;
using UnityEngine;

namespace Code.Interactable
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interactor : DetectController, IModule
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private DetectorDataSO detectorData;
        [SerializeField] private float interactCheckRadius = 1f;
        [SerializeField] private string interactUIText = "상호작용";
        
        private Transform _ownerTrm;
        private IInteractable _curInteractableItem;
        private HashSet<IInteractable> _canInteractableItems;
        
        public void Initialize(ModuleOwner owner)
        {
            _ownerTrm = owner.transform;
            _canInteractableItems = new HashSet<IInteractable>();
            
            var collider = GetComponent<SphereCollider>();
            collider.radius = interactCheckRadius;
            collider.isTrigger = true;
        }

        public bool Interact(out IInteractable interactable)
        {
            interactable = GetCurrentInteractableItem();
            _curInteractableItem = interactable;
            
            if (_curInteractableItem == null) return false;
            
            _curInteractableItem?.Interact(_ownerTrm);
            SetSelectItem(_curInteractableItem, false);
            interactable = _curInteractableItem;

            _canInteractableItems.Remove(_curInteractableItem);
            _curInteractableItem = null;
            
            return (interactable != null);
        }

        public IInteractable GetCurrentInteractableItem()
        {
            var contexts = Detect(detectorData);
            var detectContexts = contexts as DetectContext[] ?? contexts.ToArray();
            IInteractable currentItem = null;
            float currentMinDistance = int.MaxValue;

            if (!detectContexts.Any()) return null;
            
            foreach (var context in detectContexts)
            {
                IInteractable target = context.Collider.transform.GetComponent<IInteractable>();
                if (target == null) continue;
                
                _canInteractableItems.Add(target);

                float targetDistance = GetOwnerToTargetDistance(target.Transform, _ownerTrm);
                
                if (targetDistance >= currentMinDistance) continue;
                
                currentMinDistance = targetDistance;
                currentItem = target;
            }

            return currentItem;
        }

        private void SetSelectItem(IInteractable target, bool isSelect)
        {
            GameEvent evt = isSelect 
                ? UIEvents.InteractionShowEvent.Initializer(interactUIText, target.Transform, 0.8f) 
                : UIEvents.InteractionHideEvent.Initializer(target.Transform);
            
            uiChannel.RaiseEvent(evt);
            target?.Select(isSelect);
        }
        
        private void SetItemsSelect()
        {
            _curInteractableItem = GetCurrentInteractableItem();
            if(_curInteractableItem != null)
                SetSelectItem(_curInteractableItem, true);

            foreach (var target in _canInteractableItems)
            {
                if (_curInteractableItem == target) continue;
                SetSelectItem(target, false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            SetItemsSelect();
        }
        
        private void OnTriggerStay(Collider other)
        {
            SetItemsSelect();
        }

        private void OnTriggerExit(Collider other)
        {
            var target = other.GetComponent<IInteractable>();
            SetSelectItem(target, false);
            _canInteractableItems.Remove(target);
        }

        private float GetOwnerToTargetDistance(Transform owner, Transform target)
        {
            return Vector3.Distance(owner.position, target.position);
        }
    }
}