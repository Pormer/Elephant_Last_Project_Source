using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using UnityEngine;

namespace Code.Detectors
{
    public class TargetDetector : DetectController
    {
        public bool CheckTargetDetected(DetectorDataSO detectorData, out HashSet<GameObject> results)
        {
            results = new HashSet<GameObject>();

            var contexts = Detect(detectorData);

            var detectContexts = contexts as DetectContext[] ?? contexts.ToArray();
            if(!detectContexts.Any()) return false;

            foreach (var context in detectContexts)   
            {
                results.Add(context.Collider.gameObject);
            }
            
            return true;
        }
    }
}