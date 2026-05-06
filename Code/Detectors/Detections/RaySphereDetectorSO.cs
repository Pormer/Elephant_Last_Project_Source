using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors.Detections
{
    [CreateAssetMenu(fileName = "RaySphereDetectorData", menuName = "SO/Detector/RaySphere", order = 0)]
    public class RaySphereDetectorSO : DetectorSO
    {
        private RaycastHit[] _raycastHits;

        public override void SetDetectorData(DetectorDataSO detectorData)
        {
            base.SetDetectorData(detectorData);
            _raycastHits = new RaycastHit[detectorData.detectCount];
        }

        public override IEnumerable<DetectContext> Detect(Vector3 position, Quaternion rotation = default)
        {
            int cnt = Physics.SphereCastNonAlloc(position, _detectorData.radius, _detectorData.direction, _raycastHits, _detectorData.distance, _detectorData.detectLayerMask);
            var results = _raycastHits.Take(cnt).Select(ray => new DetectContext(ray.collider, ray.point, ray.normal));
            return results;
        }
    }
}