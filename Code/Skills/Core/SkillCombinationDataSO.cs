using Code.Souls.Core;
using UnityEngine;

namespace Code.Skills.Core
{
    [CreateAssetMenu(fileName = "Skill combination data", menuName = "SO/Skill/CombinationData", order = 0)]
    public class SkillCombinationDataSO : ScriptableObject
    {
        public SoulDataSO firstSoul;
        public SoulDataSO secondSoul;
        public SkillDataSO combinationSkill;

        public bool TryGetSkill(SoulDataSO first, SoulDataSO second, out SkillDataSO resultSkill)
        {
            bool isEqual = (firstSoul.Equals(first) && secondSoul.Equals(second)) || (secondSoul.Equals(first) && firstSoul.Equals(second));
            resultSkill = isEqual ? combinationSkill : null;
            return isEqual;
        }
    }
}