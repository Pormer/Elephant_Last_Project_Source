using System;
using System.Collections.Generic;
using Code.Combat;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Code.Enemies.Core
{
    [Provide]
    public class EnemyManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private float randomOffset = 0.8f;
        [SerializeField] private PoolItemSO enemyPoolItem;
        [SerializeField] private PoolManagerSO poolManager; 
        
        [Inject] private BattleManager _battleManager;

        private List<Enemy> enemyList = new List<Enemy>();

        public Enemy SpawnEnemy(EnemySpawnData spawnData)
        {
            var enemy = poolManager.Pop(enemyPoolItem) as Enemy;
            
            Vector3 offset = new Vector3(
                Random.Range(-randomOffset, randomOffset), 0,
                Random.Range(-randomOffset, randomOffset));
            enemy.transform.position = spawnData.spawnPos + offset;
            enemy.gameObject.name = spawnData.enemyData.objName;
            
            enemy.SetUpData(spawnData.enemyData);
            enemy.SetBattleManager(_battleManager);

            return enemy;
        }
        
        
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] private EnemySpawnData testData;
        [SerializeField] private int testSpawnCount = 1;
        [ContextMenu("Spawn Test")]
        public void TestSpawnEnemy()
        {
            for (int i = 0; i < testSpawnCount; i++)
                SpawnEnemy(testData);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(testData.spawnPos, 0.2f);
        }

        private void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                TestSpawnEnemy();
            }
        }
#endif
    }
}