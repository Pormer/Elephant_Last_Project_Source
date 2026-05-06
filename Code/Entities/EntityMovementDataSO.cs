using UnityEngine;

namespace Code.Entities
{
    [CreateAssetMenu(fileName = "EntityMovementData", menuName = "SO/Entity/MovementData", order = 0)]
    public class EntityMovementDataSO : ScriptableObject
    {
        // 모든 엔티티의 움직임이 비슷하게 작동하기 위해서
        // 엔티티가 베이스로 사용 할 Movement 데이터이다.
        [Header("Physics")]
        public float moveSpeed = 3.0f;      // 베이스 이동속도
        public float jumpForce = 4.0f;      // 기본 점프 파워
        public float gravity = 9.81f;       // 기본 중력
        
        // 넉백 시 바닥에 튕기는 것에 대한 데이터
        [Header("Bounce")]
        public float boundThreshold = 2.0f; // 넉백 도중 바닥에 닿을 경우 튕겨야 하는 최소속도
        public float bounceForceX = 0.75f; 
        public float bounceForceY= 0.5f; 
    }
}