using System.Collections.Generic;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using UnityEngine;

namespace Code.Detectors
{
    /// <summary>
    /// DetectorSO를 사용하기 위한 연결체 컴포넌트
    /// 직접적인 세팅은 Manager, 감지는 DetectorSO가 함
    /// </summary>
    public class DetectController : MonoBehaviour
    {
        protected virtual void Awake() { }

        public void SetDetectorTransform(DetectorDataSO detectorData)
        {
            transform.localPosition = detectorData.localPosition;
            transform.localRotation = Quaternion.Euler(detectorData.localRotation);
        }

        public IEnumerable<DetectContext> Detect(DetectorDataSO detectorData)
        {
            SetDetectorTransform(detectorData);
            DetectorSO detector = DetectorManager.Instance.GetDetector(detectorData);
            return detector.Detect(transform.position, transform.rotation);
        }

#if UNITY_EDITOR
        [Tooltip("오른쪽을 보고 있다는 기준(Rotation 적용 안된)"), SerializeField]
        private DetectorDataSO testDetectorData;

        private void OnDrawGizmos()
        {
            if (!testDetectorData || UnityEditor.EditorApplication.isPlaying) return;

            Vector3 position = transform.position + testDetectorData.localPosition;

            switch (testDetectorData.detectorShapeType)
            {
                case DetectorShapeType.OverlapSphere:
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(position, testDetectorData.radius);
                    break;
                case DetectorShapeType.OverlapBox:
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(position, testDetectorData.boxSize);
                    break;
                case DetectorShapeType.RaySphere:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(position, testDetectorData.radius);
                    break;
                case DetectorShapeType.RayBox:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(position, testDetectorData.boxSize);
                    break;
            }
        }
#endif
    }
}