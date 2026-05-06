using System.Collections.Generic;
using Code.Modules;
using UnityEngine;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyDataSetter : MonoBehaviour, IModule
    {
        private Enemy _enemy;
        private List<IEnemyDataSettable> _settableModuleList;
        
        public void Initialize(ModuleOwner owner)
        {
            _settableModuleList = new List<IEnemyDataSettable>();
            
            _enemy = owner as Enemy;
            foreach (IEnemyDataSettable enemySetter in owner.GetComponentsInChildren<IEnemyDataSettable>())
                _settableModuleList.Add(enemySetter);
        }

        public void SetEnemyData(EnemyDataSO enemyData)
        {
            foreach (IEnemyDataSettable enemySetter in _settableModuleList)
                enemySetter.SetEnemyData(enemyData);
        }
    }
}