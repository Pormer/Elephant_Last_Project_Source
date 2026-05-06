using Code.Entities.Modules;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyMovement : EntityMovement, IEnemyDataSettable
    {
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            SetUseGravity(true);
            _isBounded = false;
        }
    }
}