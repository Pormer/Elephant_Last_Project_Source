using System;
using Code.Modules;
using UnityEngine;

namespace Code.Explorations.AreaUnlocks.Impl
{
    public class BoundaryModule : MonoBehaviour, IModule
    {
        [SerializeField] private LayerMask targetLayer;
        
        private ModuleOwner _owner;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }
        
        public event Action OnBoundaryEnter;

        private void OnTriggerEnter(Collider other)
        {
            if ((targetLayer & (1 << other.gameObject.layer)) == 0) return;
            
            OnBoundaryEnter?.Invoke();
        }

        [ContextMenu("Test Invoke")]
        public void TestInvoke() => OnBoundaryEnter?.Invoke();
    }
}