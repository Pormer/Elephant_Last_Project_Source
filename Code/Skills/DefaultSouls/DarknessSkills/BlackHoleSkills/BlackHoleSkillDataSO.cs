using Code.Combat;
using Code.Combat.Attacks;
using Code.Detectors.Datas;
using Code.Skills.Core;
using Code.Summons.Explosions;
using Code.Utility.Properties;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Skills.DefaultSouls.DarknessSkills.BlackHoleSkills
{
    [CreateAssetMenu(fileName = "Black hole skill data", menuName = "SO/Skill/BlackHole", order = 0)]
    public class BlackHoleSkillDataSO : SkillDataSO
    {
        [Header("References")] 
        public GameObject blackHolePrefab;
        public GameObject blackHoleExplosionPrefab;
        public AttackDataSO pullAttackData;
        public ExplosionDataSO explosionData;
        public PropertyDataSO dissolveProperty;

        [Header("BlackHole settings")] 
        public float initSizeUpDuration;
        public float dissolveDuration;
        public float explosionDelay;
        public float sphereFinalSize; // 끌어당길 때 커지는 구의 최종 크기
        public float spherePressureDuration;
        public float sphereFinalPressureSize; // 구가 압축되는 최종 크기
        
        [Header("Attack data")]
        public Vector3 pullForce;
        public float pullDuration; 
        public float pullTerm;
    }
}