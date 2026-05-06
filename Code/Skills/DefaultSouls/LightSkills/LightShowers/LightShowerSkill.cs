using Code.Contexts.Combats;
using Code.Contexts.Summons;
using Code.Skills.Core;
using Code.Summons.Base;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShowers
{
    public class LightShowerSkill : Skill<LightShowerDataSO>
    {
        [SerializeField] private Transform showerTrm;

        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();
            
            GameObject showerObj = Instantiate(_castedData.lightShowerPrefab);
            LightShower shower =  showerObj.GetComponent<LightShower>();
            DamageContext dmgContext = CalculateDamage(_castedData.meteorAttackData, null, out _);
            SkillSummonContext summonContext = new SkillSummonContext(_entity, showerTrm.position, Vector3.zero, this, dmgContext);
            shower.SetUp(summonContext);
            shower.OnReleaseEvent.AddListener(HandleRelease);
        }

        private void HandleRelease(Summon<SkillSummonContext> shower)
        {
            shower.OnReleaseEvent.RemoveListener(HandleRelease);
            OnSkillEndEvent?.Invoke();
        }
    }
}