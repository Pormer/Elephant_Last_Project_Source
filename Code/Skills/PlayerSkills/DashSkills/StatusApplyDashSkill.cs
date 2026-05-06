using System.Collections;
using Code.Detectors;
using Code.Modules;
using Code.Statuses;
using UnityEngine;

namespace Code.Skills.PlayerSkills.DashSkills
{
    public class StatusApplyDashSkill : DashSkill
    {
        [SerializeField] private StatusApplyCaster statusApplyCaster;
        
        [SerializeField] private StatusType stunStatusType;
        [SerializeField] private float duration = 3f;

        protected override IEnumerator DashCoroutine()
        {
            statusApplyCaster.Clear();
            return base.DashCoroutine();
        }

        protected override void ApplySkillEffect()
        {
            base.ApplySkillEffect();
            
            statusApplyCaster.CastHandler(stunStatusType, duration);
        }
    }
}