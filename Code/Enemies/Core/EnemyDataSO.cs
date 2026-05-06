using System.Collections.Generic;
using Code.Combat;
using Code.Enemies.Core.Attack.AttackerData;
using Code.Enemies.Core.Colliders.Data;
using Unity.Behavior;
using Code.Entities.StatSystem;
using UnityEngine;

namespace Code.Enemies.Core
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "SO/Enemy/EnemyData", order = 0)]
    public class EnemyDataSO : ScriptableObject
    {   
        [Header("Enemy Info")]
        public string objName;                       // 적 오브젝트 이름
        public string enemyName;                     // 적 이름
        [TextArea(2, 5)] public string description;  // 적 설명
        
        [Header("Enemy Class Data")]
        public BehaviorGraph btGraph;                        // 적의 Behaviour 그래프
        public RuntimeAnimatorController controller;         // 적의 에니메이터 컨트롤러
        public EntityCombatDataSO combatData;                // 적의 전투관련 데이터 (예:타겟팅을 위한 가로 사이즈)
        public StatOverrideListSO statOverrides;             // 적의 스텟들
        public AbstractColliderDataSO colliderData;          // 적의 콜라이더 데이터
        public List<AbstactAttackerDataSO> attackerDataList; // 적의 공격 데이터 (번호는 지정해두고 외워서 사용 할 것 같음)
    }
}