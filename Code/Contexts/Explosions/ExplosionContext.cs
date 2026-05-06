using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Summons;
using Code.Modules;
using Code.Summons.Explosions;
using UnityEngine;

namespace Code.Contexts.Explosions
{
    public class ExplosionContext : SummonContext
    {
        public ExplosionDataSO ExplosionData { get; private set; }
        public IAttackSource AttackSource { get; private set; }
        
        public ExplosionContext(ModuleOwner owner, Vector3 position, Vector3 rotation, ExplosionDataSO explosionData, IAttackSource attackSource) : base(owner, position, rotation)
        {
            ExplosionData = explosionData;
            AttackSource = attackSource;
        }
    }
}