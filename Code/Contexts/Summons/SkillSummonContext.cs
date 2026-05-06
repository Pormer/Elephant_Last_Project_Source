using Code.Contexts.Combats;
using Code.Modules;
using Code.Skills;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Contexts.Summons
{
    public class SkillSummonContext : SummonContext
    {
        public Skill Skill { get; private set; }
        public DamageContext DamageContext { get; private set; }
        
        public SkillSummonContext(ModuleOwner owner, Vector3 position, Vector3 rotation, Skill skill, DamageContext dmgContext) : base(owner, position, rotation)
        {
            Skill = skill;
            DamageContext = dmgContext;
        }
    }
}