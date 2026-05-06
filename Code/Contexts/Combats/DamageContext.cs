using Code.Combat.Attacks;

namespace Code.Contexts.Combats
{
    public class DamageContext : IContext
    {
        public int Damage { get; set; }
        public IAttackSource Source { get; set; } //데미지의 출처(스킬이나 플레이어 등)
        public IAttackable Attacker { get; set; } //직접적으로 데미지를 받은 오브젝트(투사체 등)

        public AttackDataSO AttackData { get; private set; }
        public float KnockbackXDir { get; private set; }
        public bool IsSelfKnockback { get; private set; }
        public bool IsCritical { get; private set; }
        
        public DamageContext(
            int damage, IAttackSource source, IAttackable attacker, AttackDataSO attackData,
            bool isSelfKnockback = false, float knockbackXDir = 0, bool isCritical = false)
        {
            Damage = damage;
            Source = source;
            Attacker = attacker;
            IsSelfKnockback = isSelfKnockback;
            KnockbackXDir = knockbackXDir;
            IsCritical = isCritical;
            AttackData = attackData;
        }
    }
}