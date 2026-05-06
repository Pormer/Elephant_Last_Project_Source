using Code.Contexts.Combats;
using Code.Contexts.Summons;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShieldSkills
{
    public class LightShieldSkill : Skill<LightShieldSKillDataSO>
    {
        [SerializeField] private Transform shieldSpawnTrm;

        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();

            GameObject instance = Instantiate(_castedData.shieldPrefab);
            LightShield shield = instance.GetComponent<LightShield>();
            DamageContext dmgContext = CalculateDamage(_castedData.attackData, shield, out _);
            SkillSummonContext summonContext = new SkillSummonContext(_entity, shieldSpawnTrm.position, Vector3.zero, this, dmgContext);
            shield.SetUp(summonContext);
            OnSkillEndEvent?.Invoke();
        }
    }
}