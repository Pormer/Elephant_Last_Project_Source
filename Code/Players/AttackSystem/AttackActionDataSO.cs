using Code.Combat.Attacks;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Players.AttackSystem
{
    [CreateAssetMenu(fileName = "AttackActionData", menuName = "SO/Combat/AttackActionData", order = 0)]
    public class AttackActionDataSO : ScriptableObject
    {
        public AttackDataSO attackData;
        public DetectorDataSO casterData;
        public bool canContinueAttack = true;

        public Vector3 hitPoint;
    }
}