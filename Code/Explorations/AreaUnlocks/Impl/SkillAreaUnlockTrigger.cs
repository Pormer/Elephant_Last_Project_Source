using System.Collections.Generic;
using System.Linq;
using Code.Combat;
using Code.Contexts;
using Code.Contexts.Combats;
using Code.Skills;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Explorations.AreaUnlocks.Impl
{
    public class SkillAreaUnlockTrigger : DamageableAreaUnlockTrigger
    {
        [SerializeField] private List<SkillDataSO> targetSkills;

        private HashSet<SkillDataSO> _targetSkillSet;

        protected override void Awake()
        {
            base.Awake();

            _targetSkillSet = targetSkills.ToHashSet();
        }

        protected override void HandleDead()
        {
            base.HandleDead();
            
            RequestUnlock();
            Destroy(gameObject);
        }

        public override void TakeDamage(DamageContext context)
        {
            if (context.Source is Skill skill && _targetSkillSet.Contains(skill.SkillData))
            {
                _health.TakeDamage(context.Damage);
            }
        }
    }
}