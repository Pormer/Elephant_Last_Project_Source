using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors.Detections
{
    [CreateAssetMenu(fileName = "RayBoxDetectorData", menuName = "SO/Detector/RayBox", order = 0)]
    public class RayBoxDetectorSO : DetectorSO
    {
        private RaycastHit[] _raycastHits;
        
        public override void SetDetectorData(DetectorDataSO detectorData)
        {
            base.SetDetectorData(detectorData);
            _raycastHits = new RaycastHit[detectorData.detectCount];
        }

        public override IEnumerable<DetectContext> Detect(Vector3 position, Quaternion rotation = default)
        {
            // 임시 코드
            int cnt = Physics.BoxCastNonAlloc(position, _detectorData.boxSize * 0.5f, _detectorData.direction, _raycastHits, rotation, _detectorData.distance, _detectorData.detectLayerMask);
            var results = _raycastHits.Take(cnt).Select(ray => new DetectContext(ray.collider, ray.point, ray.normal));
            return results;
        }
    }
}