using Code.Modules;
using UnityEngine;

namespace Code.Souls.FollowPets
{
    public class FollowTargetMovement : MonoBehaviour, IModule
    {
        [Header("Movement Data")]
        [SerializeField] private float smoothTime = 0.5f;
        [SerializeField] private float maxSpeed = int.MaxValue; 
        [SerializeField] private float randomOffsetValue = 0.5f;
        [SerializeField] private float stopInterval = 0.03f;

        [Header("Floating Effect")]
        public float amplitude = 0.5f;   // 위아래 흔들림 폭
        public float frequency = 1.5f;   // 흔들림 속도
        
        private Vector3 _offsetPos;
        private Vector3 _currentRandomOffset;
        private Vector3 _currentVelocity;
        
        private Transform _ownerTrm;
        private Transform _targetTrm;
        
        public void Initialize(ModuleOwner owner)
        {
            _ownerTrm = owner.transform;
        }
        
        public void SetTarget(Transform target, Vector3 offset)
        {
            _targetTrm = target; 
            _offsetPos = offset;
        }

        private void FixedUpdate()
        {
            FollowTarget(_targetTrm);
        }

        private void FollowTarget(Transform target)
        {
            if (!target) return;

            Vector3 targetPos = target.TransformPoint(_offsetPos + _currentRandomOffset);

            float floatingY = Mathf.Sin(Time.time * frequency) * amplitude;
            targetPos.y += floatingY;
            
            _ownerTrm.position = Vector3.SmoothDamp(_ownerTrm.position, targetPos, 
                ref _currentVelocity, smoothTime, maxSpeed);

            if ((_ownerTrm.position - targetPos).magnitude > randomOffsetValue * 2)
            {
                int targetY = _currentVelocity.x > 0 ? 0 : 180;
                _ownerTrm.eulerAngles = new Vector3(0, targetY, 0);
            }
            
            if (ApproximatelyVector(_currentVelocity, Vector3.zero))
            {
                RandomOffsetPosition();
            }
        }

        private void RandomOffsetPosition()
        {
            _currentRandomOffset.x = Random.Range(-randomOffsetValue, randomOffsetValue);
            _currentRandomOffset.y = Random.Range(-randomOffsetValue, randomOffsetValue);
            _currentRandomOffset.z = Random.Range(-randomOffsetValue, randomOffsetValue);
        }

        private bool ApproximatelyVector(Vector3 aVec, Vector3 bVec)
        {
            float interval = Mathf.Abs(aVec.sqrMagnitude - bVec.sqrMagnitude);
            return interval < stopInterval;
        }
    }
}