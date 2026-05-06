using UnityEngine;

namespace Code.Combat
{
    [CreateAssetMenu(fileName = "EntityCombatDataSO", menuName = "SO/Entity/EntityCombatData", order = 0)]
    public class EntityCombatDataSO : ScriptableObject
    {
        // 적이 엔티티를 공격 할 때 이동 할 위치 계산용
        // 혹시 용병같은걸 만들어서 적을 플레이어 외의 엔티티를 공격할 수도 있으니까 일단 이렇게 만들어둠
        public float size;
    }
}