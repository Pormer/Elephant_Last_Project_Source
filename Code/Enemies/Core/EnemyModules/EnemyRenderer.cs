using Code.Entities.Modules;
using Code.Utility.Properties;
using Code.Utility.Properties.Shaders;
using UnityEngine;

namespace Code.Enemies.Core.EnemyModules
{
    public class EnemyRenderer : EntityRenderer, IEnemyDataSettable
    {
        [SerializeField] private PropertyDataSO isDeadProperty;
        [SerializeField] private ShaderPropertyManagerSO propertyManager;
        
        public void SetEnemyData(EnemyDataSO enemyData)
        {
            propertyManager.SetProperty(Material, isDeadProperty, 0);
            visual.enabled = true;
        }
    }
}