using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors.Detections
{
    [CreateAssetMenu(fileName = "OverlapSphereDetectorData", menuName = "SO/Detector/OverlapSphere", order = 0)]
    public class OverlapSphereDetectorSO : DetectorSO
    {
        private Collider[] _colliders;

        public override void SetDetectorData(DetectorDataSO detectorData)
        {
            base.SetDetectorData(detectorData);
            _colliders = new Collider[detectorData.detectCount];
        }

        public override IEnumerable<DetectContext> Detect(Vector3 position, Quaternion rotation = default)
        {
            var cnt = Physics.OverlapSphereNonAlloc(position, _detectorData.radius, _colliders, _detectorData.detectLayerMask);
            var results = _colliders.Take(cnt).Select(col => new DetectContext(col, col.bounds.center));
            return results;
        }
    }
}