using Code.Entities.Modules;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyAnimator : EntityAnimator, IEnemyDataSettable
    {
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            _animator.runtimeAnimatorController = enemyData.controller;
            _animator.Rebind();
        }
    }
}