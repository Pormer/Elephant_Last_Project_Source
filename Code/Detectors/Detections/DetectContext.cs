using Code.Contexts;
using UnityEngine;

namespace Code.Detectors.Detections
{
    public struct DetectContext : IContext
    {
        public Collider Collider { get; private set; }
        public Vector3 Point { get; private set; }  
        public Vector3 Normal { get; private set; }
    
        public DetectContext(Collider col, Vector3 point, Vector3 normal = default)
        {
            Collider = col;
            Point = point;
            Normal = normal;
        }
    }
}