using Code.Combat;
using Code.Combat.Attacks;
using Code.Contexts.Summons;
using Code.Modules;
using UnityEngine;

namespace Code.Contexts.Projectiles
{
    public class ProjectileContext : SummonContext
    {
        public Vector3 Dir { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public AttackDataSO AttackData { get; set; }
        
        public ProjectileContext(ModuleOwner owner, Vector3 position, Vector3 rotation, Vector3 dir, float speed, int damage, AttackDataSO attackData) : base(owner, position, rotation)
        {
            Dir = dir;
            Speed = speed;
            Damage = damage;
            AttackData = attackData;
        }
    }
}