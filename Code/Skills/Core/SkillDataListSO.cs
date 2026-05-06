using System.Collections.Generic;
using UnityEngine;

namespace Code.Skills.Core
{
    [CreateAssetMenu(fileName = "Skill data list", menuName = "SO/Skill/List", order = 0)]
    public class SkillDataListSO : ScriptableObject
    {
        public List<SkillDataSO> values = new List<SkillDataSO>();
    }
}