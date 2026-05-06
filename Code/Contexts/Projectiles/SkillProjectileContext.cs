using Code.Combat;
using Code.Combat.Attacks;
using Code.Modules;
using Code.Skills;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Contexts.Projectiles
{
    public class SkillProjectileContext : ProjectileContext
    {
        public Skill Skill { get; set; }
        
        public SkillProjectileContext(ModuleOwner owner, Vector3 position, Vector3 rotation, Vector3 dir, float speed, int damage, AttackDataSO attackData, Skill skill) : base(owner, position, rotation, dir, speed, damage, attackData)
        {
            Skill = skill;
        }
    }
}