using Code.Combat;
using Code.Combat.Attacks;

namespace Code.Enemies.Core.Attack.Attacker
{
    public class SummonAttacker : AbstractAttacker
    {
        

        public override void StartAttack()
        {
            //     PoolingSummon<SummonContext> game = PoolManagerMono.Instance.Pop<PoolingSummon<SummonContext>>(_summonAttackDataList[_attackIndex].summonItem);
            //     Vector3 dir = new Vector3((_owner.transform.rotation.y == 0 ? -1 : 1), 0, 0);
            //     SummonContext context = new SummonContext(_owner, pos, Vector3.zero, dir, 0, 0, null);
            //     game.SetUp(context);
        }

        public override void EndAttack()
        {
            
        }
    }
}