using Code.Combat;
using Code.Contexts.Combats;
using Code.Contexts.Summons;
using Code.Entities.StatSystem;
using Code.Modules;
using Code.Summons;
using Code.Summons.Base;

namespace Code.Skills.Core
{
    public abstract class SkillSummon<TContext, TData> : Summon<TContext> where TContext : SkillSummonContext where TData : SkillDataSO
    {
        protected Skill _skill;
        protected TData _castedData;
        protected DamageContext _damageContext;

        public override void SetUp(TContext context)
        {
            base.SetUp(context);
            
            _skill = context.Skill;
            _castedData = _skill.SkillData as TData;
            _damageContext = context.DamageContext;
        }
    }
}