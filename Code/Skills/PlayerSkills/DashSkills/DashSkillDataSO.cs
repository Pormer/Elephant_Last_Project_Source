using Code.Skills.Core;
using UnityEngine;

namespace Code.Skills.PlayerSkills.DashSkills
{
    [CreateAssetMenu(fileName = "Dash_SkillData", menuName = "SO/Skill/DashSkill", order = 0)]
    public class DashSkillDataSO : SkillDataSO
    {
        public float dashDuration = 0.25f;
        public float dashSpeed = 25.0f;
    }
}