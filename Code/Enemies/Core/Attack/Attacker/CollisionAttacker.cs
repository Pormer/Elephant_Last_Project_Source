namespace Code.Enemies.Core.Attack.Attacker
{
    public class CollisionAttacker : CasterAttacker
    {
        private bool _isCasting;

        public override void StartAttack()
        {
            _isCasting = true;
            damageCaster.StartCasting();
        }

        private void Update()
        {
            if (_isCasting == false) return;

            CastDamage();
        }

        public override void EndAttack()
        {
            _isCasting = false;
        }
    }
}