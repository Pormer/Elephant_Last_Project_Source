using System;
using System.Collections.Generic;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Detectors.Detections
{
    public abstract class DetectorSO : ScriptableObject, ICloneable
    {
        [field:SerializeField] public DetectorShapeType ShapeType { get; private set; }
        
        protected DetectorDataSO _detectorData;
        
        public virtual void SetDetectorData(DetectorDataSO detectorData)
        {
            _detectorData = detectorData;
        }
        
        public abstract IEnumerable<DetectContext> Detect(Vector3 position, Quaternion rotation = default);
        
        public object Clone()
        {
            return Instantiate(this); //자기자신을 복제해서 준다.
        }
    }
}