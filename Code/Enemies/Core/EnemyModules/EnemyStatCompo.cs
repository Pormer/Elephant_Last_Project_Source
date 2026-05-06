using Code.Entities.StatSystem;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyStatCompo : EntityStatCompo, IEnemyDataSettable
    {
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            SetBaseStatOverrides(enemyData.statOverrides);
        }
    }  
}