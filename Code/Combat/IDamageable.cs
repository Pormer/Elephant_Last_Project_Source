using Code.Contexts.Combats;

namespace Code.Combat
{
    public interface IDamageable
    {
        public void TakeDamage(DamageContext context);
    }
}