using System;
using UnityEngine;

namespace Code.Explorations.AreaUnlocks.Impl
{
    public class BoundaryAreaUnlockTrigger : AreaUnlockTrigger
    {
        private BoundaryModule _boundary;
        
        protected override void Awake()
        {
            base.Awake();

            _boundary = GetModule<BoundaryModule>();
            Debug.Assert(_boundary != null, $"Boundary is null : {gameObject.name}");
            
            _boundary.OnBoundaryEnter += HandleBoundaryEnter;
        }

        private void OnDestroy()
        {
            _boundary.OnBoundaryEnter -= HandleBoundaryEnter;
        }

        private void HandleBoundaryEnter()
        {
            RequestUnlock();
        }
    }
}