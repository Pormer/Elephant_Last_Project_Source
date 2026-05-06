using System.Collections.Generic;
using Code.Enemies.Core.Colliders.Data;
using Code.Enemies.Core.Colliders.Setter;
using Code.Modules;
using UnityEngine;

namespace Code.Enemies.Core.Colliders
{
    public class EnemyColliderInstaller : MonoBehaviour, IModule, IEnemyDataSettable
    {
        private Dictionary<ColliderType, AbstractColliderSetter> _setterDict;
        private Enemy _enemy;
        
        public void Initialize(ModuleOwner owner)
        {
            _enemy = owner as Enemy;
            _setterDict = new Dictionary<ColliderType, AbstractColliderSetter>();
            
            Debug.Assert(_enemy != null, $"{gameObject.name} : owner is not Enemy");
            
            foreach (AbstractColliderSetter setter in GetComponentsInChildren<AbstractColliderSetter>())
                _setterDict.Add(setter.ColliderType, setter);
        }

        public void SetEnemyData(EnemyDataSO enemyData)
        {
            DisableColliders();
            
            AbstractColliderDataSO colliderData = enemyData.colliderData;
            Debug.Assert(colliderData != null, $"{gameObject.name} : ColliderData is null");

            if (_setterDict.TryGetValue(colliderData.colliderType, out AbstractColliderSetter setter))
            {
                setter.SetColliderData(colliderData);
            }
        }

        public void DisableColliders()
        {
            foreach (AbstractColliderSetter setter in _setterDict.Values)
                setter.DisableCollider();
        }
    }
}