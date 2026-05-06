using Code.Contexts.Combats;
using Code.Contexts.Summons;
using Code.Skills.Core;
using Code.Summons.Base;
using UnityEngine;

namespace Code.Skills.DefaultSouls.DarknessSkills.BlackHoleSkills
{
    public class BlackHoleSkill : Skill<BlackHoleSkillDataSO>
    {
        [SerializeField] private Transform spawnTrm;
        
        protected override void ExecuteSkill()
        {
            base.ExecuteSkill();

            GameObject instance = Instantiate(_castedData.blackHolePrefab);
            BlackHole blackHole = instance.GetComponent<BlackHole>();
            DamageContext dmgContext = CalculateDamage(_castedData.pullAttackData, blackHole, out _);
            SkillSummonContext summonContext = new SkillSummonContext(_entity, spawnTrm.position, Vector3.zero, this, dmgContext);
            blackHole.SetUp(summonContext);
            blackHole.OnReleaseEvent.AddListener(HandleBlackHoleReleased);
        }

        private void HandleBlackHoleReleased(Summon<SkillSummonContext> blackHole)
        {
            blackHole.OnReleaseEvent.RemoveListener(HandleBlackHoleReleased);
            OnSkillEndEvent?.Invoke();
        }
    }
}