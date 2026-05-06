using Code.Combat.Attacks;
using Code.Detectors.Datas;
using Code.Skills.Core;
using Code.Utility.Properties;
using UnityEngine;

namespace Code.Skills.DefaultSouls
{
    [CreateAssetMenu(fileName = "Strong paunch data", menuName = "SO/Skill/StrongPaunch", order = 0)]
    public class StrongPaunchSkillDataSO : SkillDataSO
    {
        [ColorUsage(true, true)] public Color emissionColor;
        public DetectorDataSO damageCasterData;
        public MovementAttackDataSO attackData;
        public PropertyDataSO emissionProperty;
    }
}