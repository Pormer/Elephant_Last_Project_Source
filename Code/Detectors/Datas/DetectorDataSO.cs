using UnityEngine;

namespace Code.Detectors.Datas
{
    [CreateAssetMenu(fileName = "DetectorData", menuName = "SO/Detector/DetectorData", order = 0)]
    public class DetectorDataSO : ScriptableObject
    {
        [Header("Info")]
        public DetectorShapeType detectorShapeType; // 감지 모양, 방식
        public LayerMask detectLayerMask;           // 감지 레이어
        public int detectCount;                     // 감지 카운트
        
        [Header("Pose")]
        public Vector3 localPosition;  // 부모기준 감지 위치
        public Vector3 localRotation;  // 부모기준 감지 회전치
        public Vector3 direction;      // Ray 사용 할 거 생각해서 방향
        
        [Header("Shape size")]
        public float radius;     // 원 크기
        public Vector3 boxSize;  // 상자 크기
        public float distance;   // Ray 길이
    }
}