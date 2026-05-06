using Code.Entities.StatSystem;
using Code.Modules;
using Code.Statuses;
using UnityEngine;

namespace Code.Players.Statuses
{
    public class PlayerSlowStatus : Status
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private float debuffValue = -0.3f;
        private EntityStatCompo _statCompo;
        
        public override void Initialize(ModuleOwner owner)
        {
            _statCompo = owner.GetModule<EntityStatCompo>();
            Debug.Assert(_statCompo != null, $"Stat component is null : {gameObject.name}");
        }

        public override void Apply(float duration)
        {
            _statCompo.AddModifier(moveSpeedStat, this, debuffValue);
        }

        public override void End()
        {
            _statCompo.RemoveModifier(moveSpeedStat, this);
        }
    }
}