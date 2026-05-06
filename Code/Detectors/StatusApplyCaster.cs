using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using Code.Statuses;
using UnityEngine;

namespace Code.Detectors
{
    public class StatusApplyCaster : DetectController
    {
        [SerializeField] private DetectorDataSO detectorData;
        protected HashSet<IStatusHandler> _handlers;

        protected override void Awake()
        {
            base.Awake();
            _handlers = new HashSet<IStatusHandler>();
        }

        public virtual void Clear()
        {
            _handlers.Clear();
        }

        public bool CastHandler(StatusType type, float duration)
        {
            var contexts = Detect(detectorData);
            var detectContexts = contexts as DetectContext[] ?? contexts.ToArray();
            
            foreach (var context in detectContexts)  
            {
                Transform target = context.Collider.transform;
                StatusApply(target, type, duration);
            }

            return detectContexts.Any();
        }

        public bool StatusApply(Transform target, StatusType type, float duration)
        {
            if (target.TryGetComponent(out IStatusHandler statusHandler))
            {
                if (_handlers.Contains(statusHandler)) return false;
                
                statusHandler.ApplyStatus(type, duration);
                _handlers.Add(statusHandler);
                
                return true;
            }

            return false;
        }
    }
}