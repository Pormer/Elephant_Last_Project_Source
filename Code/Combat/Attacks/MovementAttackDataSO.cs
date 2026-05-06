using UnityEngine;

namespace Code.Combat.Attacks
{
    [CreateAssetMenu(fileName = "Movement_AttackData", menuName = "SO/Combat/Movement AttackData", order = 0)]
    public class MovementAttackDataSO : AttackDataSO
    {
        [Header("Movement")] 
        public Vector3 startAttackForce; //오른쪽을 바라보고 있다는 기준 공격시 움직일량
        public Vector3 successAttackForce; //오른쪽을 바라보고 있다는 기준 공격시 움직일량
    }
}