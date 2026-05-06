using Code.Contexts.Projectiles;
using Code.Skills.Core;

namespace Code.Summons.Projectiles
{
    public abstract class SkillProjectile : Projectile<SkillProjectileContext>
    {
        protected Skill _skill;

        public override void SetUp(SkillProjectileContext context)
        {
            base.SetUp(context);

            _skill = context.Skill;
        }
    }
}