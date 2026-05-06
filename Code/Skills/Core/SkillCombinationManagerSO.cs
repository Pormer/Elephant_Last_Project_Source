using System.Collections.Generic;
using Code.Souls.Core;
using UnityEngine;

namespace Code.Skills.Core
{
    [CreateAssetMenu(fileName = "Skill combination manager", menuName = "SO/Skill/CombinationManager", order = 0)]
    public class SkillCombinationManagerSO : ScriptableObject
    {
        public List<SkillCombinationDataSO> combinationDataList;

        public SkillDataSO GetCombinationSkill(SoulDataSO firstSoul, SoulDataSO secondSoul)
        {
            SkillDataSO result = null;
            
            foreach (SkillCombinationDataSO combinationData in combinationDataList)
            {
                if (combinationData.TryGetSkill(firstSoul, secondSoul, out result)) break;
            }
            
            return result;
        }
    }
}