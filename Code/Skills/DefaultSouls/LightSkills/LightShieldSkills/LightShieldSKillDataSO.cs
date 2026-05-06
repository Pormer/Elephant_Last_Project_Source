using Code.Combat;
using Code.Combat.Attacks;
using Code.Skills.Core;
using Code.Utility.Properties;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShieldSkills
{
    [CreateAssetMenu(fileName = "Light shield", menuName = "SO/Skill/LightShield", order = 0)]
    public class LightShieldSKillDataSO : SkillDataSO
    {
        public GameObject shieldPrefab;
        public float shieldUnfoldDuration;
        public float moveAmount;
        public float moveDuration;
        public PropertyDataSO dissolveProperty;
        public float dissolveDuration;
        public AttackDataSO attackData;
    }
}