using System;

namespace Code.Skills.Core
{
    [Serializable]
    public struct SkillKeyMatchData
    {
        public SkillDataSO targetSkill;
        public SkillKeyType keyType;
    }
}