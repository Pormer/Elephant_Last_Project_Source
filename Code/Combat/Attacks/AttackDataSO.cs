using Code.Combat.KnockBacks;
using UnityEngine;

namespace Code.Combat.Attacks
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public string attackName;
        
        [Header("Attack")]
        public float damageMultiplier = 1f;
        public float damageIncrease = 0;
        
        public AttackType attackType;
        
        [Header("Knockback")]
        public KnockBackDataSO knockBackData;
    }
}