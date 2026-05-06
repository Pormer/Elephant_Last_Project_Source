using System.Collections.Generic;
using System.Linq;
using Code.Detectors.Datas;
using Code.Detectors.Detections;
using Code.Utility;
using UnityEngine;

namespace Code.Detectors
{
    [DefaultExecutionOrder(-10)]
    public class DetectorManager : MonoBehaviour
    {
        [SerializeField] private List<DetectorSO> detectorList;
        [SerializeField] private List<DetectorDataSO> detectorDataList;
        
        private Dictionary<DetectorShapeType, DetectorSO> _baseDetectorDict;
        private Dictionary<DetectorDataSO, DetectorSO> _cloneDetectorDict;

        private static DetectorManager _instance;
        public static DetectorManager Instance => _instance;
        
        private void Awake()
        {
            if (_instance != null)
                Debug.LogWarning("DetectorManager is already existing");
            else
                _instance = this;

            _cloneDetectorDict = new Dictionary<DetectorDataSO, DetectorSO>();
            _baseDetectorDict = detectorList.ToDictionary(so => so.ShapeType,  so => so);
            SetupDetector();
        }

        private void SetupDetector()
        {
            if (detectorDataList.Count == 0)
            {
                Debug.LogWarning("DetectData is not found");
                return;
            }
            
            foreach (DetectorDataSO detectorData in detectorDataList)
            {
                // 모양에 맞는 Detector가 있다면
                if (_baseDetectorDict.TryGetValue(detectorData.detectorShapeType, out DetectorSO baseDetector))
                {
                    // 타입에 맞는 디텍터를 복사, 세팅 후 딕셔러니 저장
                    DetectorSO detector = baseDetector.Clone() as DetectorSO;
                    detector.SetDetectorData(detectorData);
                    _cloneDetectorDict.Add(detectorData, detector);
                }
            }
        }

        public DetectorSO GetDetector(DetectorDataSO detectorData)
        {
            DetectorSO detector = _cloneDetectorDict[detectorData];
            Debug.Assert(detector != null, $"{detectorData} is not found");
            return detector;
        }

        #if UNITY_EDITOR
        
        [ContextMenu("Detector Data all Find")]
        private void GetAllDetectorData()
        {
            detectorDataList = EditorAssetsFinder.GetAllAssetsOfType<DetectorDataSO>();
        }
        
        #endif
        
    }
}