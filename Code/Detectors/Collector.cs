using System.Linq;
using Code.Collectibles.CollectableItems;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using Code.Modules;
using UnityEngine;

namespace Code.Detectors
{
    [RequireComponent(typeof(SphereCollider))]
    public class Collector : DetectController, IModule
    {
        [SerializeField] private DetectorDataSO detectorData;
        [SerializeField] private float collectCheckRadius = 1f;
        
        private Transform _ownerTrm;
        private SphereCollider _collider;
        
        public void Initialize(ModuleOwner owner)
        {
            _ownerTrm = owner.transform;
            
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _collider.radius = collectCheckRadius;
        }

        public virtual bool ApplyCollecting(Transform target)
        {
            if (target.TryGetComponent(out ICollectable collectable))
            {
                collectable.StartCollecting(_ownerTrm);
                return true;
            }
            
            return false;
        }

        public bool TryCollect()
        {
            var contexts = Detect(detectorData);

            var detectContexts = contexts as DetectContext[] ?? contexts.ToArray();
            foreach (var context in detectContexts)
            {
                Transform target = context.Collider.transform;
                ApplyCollecting(target);
            }

            return detectContexts.Any();  
        }

        private void OnTriggerEnter(Collider other)
        {
            TryCollect();
        }
        
        private void OnTriggerStay(Collider other)
        {
            TryCollect();
        }
    }
}