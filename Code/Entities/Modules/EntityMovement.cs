using System;
using Code.Detectors;
using Code.Detectors.Datas;
using Code.Entities.StatSystem;
using Code.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities.Modules
{
    public class EntityMovement : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private EntityMovementDataSO movementData;
        [SerializeField] private StatSO speedMultiplierStatData;
        
        [SerializeField] private Rigidbody rbCompo;
        [SerializeField] private TargetDetector groundDetector;
        [SerializeField] private DetectorDataSO groundDetectorData;
        [SerializeField] private LayerMask whatIsGround;
        
        private EntityStatCompo _statCompo;
        private EntityAnimator _entityAnimator;

        protected bool _isBounded;
        private float _statMoveSpeedMultiplier;
        private float _moveSpeedMultiplier = 1;
        private Vector3 _velocityOffset;
        
        private readonly int _moveSpeedHash = Animator.StringToHash("MOVE_SPEED");
        
        public Entity Entity { get; private set; }
        public bool CanManualMove { get; set; } = true;
        public bool UseGravity { get; private set; } = true;
        public Vector3 CurrentMoveDirection { get; private set; }
        public bool IsGrounded => groundDetector?.CheckTargetDetected(groundDetectorData, out _) ?? false;
        public float VerticalVelocity => rbCompo.linearVelocity.y;

        public UnityEvent OnLandedEvent; 
        public UnityEvent OnStopBoundEvent;
        public event Action<Vector3> OnMoveEvent;
        
        public void Initialize(ModuleOwner owner)
        {
            Entity = owner as Entity;
            _statCompo = owner.GetModule<EntityStatCompo>();
            _entityAnimator = owner.GetModule<EntityAnimator>();
            
            Debug.Assert(Entity != null, "owner is not Entity");
            Debug.Assert(_statCompo != null, "stat component is null");
            Debug.Assert(_entityAnimator != null, "entity animator is null");
            Debug.Assert(rbCompo != null, "rigidbody is null");
        }
        
        public void AfterInitialize()
        {
            _statMoveSpeedMultiplier = _statCompo.SubscribeStat(speedMultiplierStatData, HandleSpeedMultiplierChange, 1);
        }
        
        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(speedMultiplierStatData, HandleSpeedMultiplierChange);
        }

        private void HandleSpeedMultiplierChange(StatSO stat, float currentValue, float previousValue)
        {
            _statMoveSpeedMultiplier = currentValue;
            _entityAnimator.SetParam(_moveSpeedHash, currentValue);
        }

        private void FixedUpdate()
        {
            ApplyGravity();

            MoveUpdate();
        }

        private void ApplyGravity()
        {
            if (UseGravity == false)
            {
                Vector3 velocity = rbCompo.linearVelocity;
                velocity.y = 0f;
                rbCompo.linearVelocity = velocity;
                return;
            }
            
            // 땅에 있다면 Y 속도를 -1로 고정
            if (IsGrounded)
            {
                if (Entity.CheckIsAirborne())
                    BoundEntity();
                else
                {
                    Vector3 velocity = rbCompo.linearVelocity;
                    velocity.y = -1f;
                    rbCompo.linearVelocity = velocity;
                }
            }
            else
                rbCompo.AddForce(Vector3.down * (movementData.gravity * Time.fixedDeltaTime), ForceMode.Impulse);
        }
        
        private void MoveUpdate()
        {
            Vector3 velocity;

            if (CanManualMove)
            {
                velocity = CurrentMoveDirection * (movementData.moveSpeed * _statMoveSpeedMultiplier * _moveSpeedMultiplier);
                velocity.y = rbCompo.linearVelocity.y;
            }
            else
                velocity = rbCompo.linearVelocity;
                
            velocity += _velocityOffset;
                
            if (IsGrounded)
                velocity = FollowFloor(velocity);
            
            rbCompo.linearVelocity = velocity;
        }
        
        private void BoundEntity()
        {
            // 바닥에 닿았는데 위로 올라가는 중이면 리턴
            if (VerticalVelocity > 0f)
                return;
            
            if (_isBounded == false && VerticalVelocity < 0f)
            {
                _isBounded = true;
                
                Vector3 velocity = new Vector3(
                    movementData.bounceForceX * Mathf.Sign(rbCompo.linearVelocity.x), movementData.bounceForceY);
                rbCompo.linearVelocity = velocity;
                OnLandedEvent?.Invoke();
            }
            else
            {
                SetCanManualMove(true);
                rbCompo.linearVelocity = Vector3.zero;
                OnStopBoundEvent?.Invoke();
                
                _isBounded = false;
            }
        }

        private Vector3 FollowFloor(Vector3 moveVelocity)
        {
            const float groundRayLength = 50f;
            
            // 엔티티 중앙 바닥 위치
            // 바닥을 뚫는 경우 방지를 위해 약간 올림
            Vector3 rayOrigin = Entity.transform.position + Vector3.up * 0.3f;
            
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayLength, whatIsGround))
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                return slopeRotation * moveVelocity;
            }
        
            // 바닥이 감지되지 않으면 원래 방향 그대로 리턴
            return moveVelocity;
        }
        
        public void SetMoveDirection(Vector2 inputDir)
        {
            CurrentMoveDirection = new Vector3(inputDir.x, 0, inputDir.y);
            OnMoveEvent?.Invoke(CurrentMoveDirection);
        }
        
        public void SetMoveDirection(Vector3 inputDir)
        {
            CurrentMoveDirection = inputDir; 
            OnMoveEvent?.Invoke(CurrentMoveDirection);
        }

        public void Knockback(Vector3 velocity)
        {
            SetCanManualMove(false);
            StopMovement();
            
            // 속도 0 되서 바닥처리되는거 방지용 Y축 임시값
            // rbCompo.linearVelocity = Vector3.up;

            if (IsGrounded == false)
                velocity += Vector3.up;
            
            // AddForce(velocity, ForceMode.VelocityChange);
            rbCompo.linearVelocity = velocity;
        }
        
        public void StopMovement()
        {
            CurrentMoveDirection = Vector3.zero;
            rbCompo.linearVelocity = Vector3.zero;
        }

        public void Jump()
        {
            StopMovement();
            AddForce(Vector3.up * movementData.jumpForce, ForceMode.VelocityChange);
        }
        
        public void JumpTo(Vector3 direction, float jumpPower)
        {
            StopMovement();
            direction.y = movementData.jumpForce + jumpPower;
            AddForce(direction, ForceMode.VelocityChange);
        }

        public void WarpTo(Vector3 targetPos)
        {
            Entity.transform.position = targetPos;
            rbCompo.position = targetPos;
        }

        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse) => rbCompo.AddForce(force, forceMode);
        public void SetSpeedMultiplier(float speed = 1f) => _moveSpeedMultiplier = speed;
        public void SetVelocityOffset(Vector3 offset) => _velocityOffset = offset;
        public void SetCanManualMove(bool canManualMove) => CanManualMove = canManualMove;
        public void SetUseGravity(bool useGravity) => UseGravity = useGravity;
    }
}