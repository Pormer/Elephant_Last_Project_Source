using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors.Detections
{
    [CreateAssetMenu(fileName = "OverlapBoxDetectorData", menuName = "SO/Detector/OverlapBox", order = 0)]
    public class OverlapBoxDetectorSO : DetectorSO
    {
        private Collider[] _colliders;

        public override void SetDetectorData(DetectorDataSO detectorData)
        {
            base.SetDetectorData(detectorData);
            _colliders = new Collider[detectorData.detectCount];
        }

        public override IEnumerable<DetectContext> Detect(Vector3 position, Quaternion rotation = default)
        {
            var cnt = Physics.OverlapBoxNonAlloc(position, _detectorData.boxSize * 0.5f, _colliders, rotation, _detectorData.detectLayerMask);
            var results = _colliders.Take(cnt).Select(col => new DetectContext(col, col.bounds.center));
            return results;
        }
    }
}