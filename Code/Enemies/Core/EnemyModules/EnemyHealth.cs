using Code.Combat;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyHealth : StatBasedHealth, IEnemyDataSettable
    {
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            MaxHealth = (int)_statCompo.SubscribeStat(healthStat, HandleMaxMaxHealthChange, MaxHealth);
            SetUpHealth(MaxHealth);
        }
    }
}