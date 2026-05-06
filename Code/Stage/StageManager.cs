using System.Collections.Generic;
using Code.Enemies.Core;
using Code.Entities;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Stage
{
    [Provide]
    public class StageManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<StageDataSO> stageDataList;
        [SerializeField] private GameEventChannelSO combatChannel;

        [Inject] private EnemyManager _enemyManager;

        private Dictionary<StageDataSO, List<Enemy>> _stageEnemyDict;
        private List<Enemy> _currentStageEnemies;

        private void Awake()
        {
            _stageEnemyDict = new Dictionary<StageDataSO, List<Enemy>>();
            
            foreach (var stageData in stageDataList)
            {
                List<Enemy> enemies = new List<Enemy>();
                foreach (var spawnData in stageData.spawnDataList)
                {
                    enemies.Add(_enemyManager.SpawnEnemy(spawnData));
                }
                
                _stageEnemyDict[stageData] = enemies;
            }
        }

        public void StartStage(StageDataSO stageData)
        {
            _currentStageEnemies = _stageEnemyDict[stageData];
            foreach (Enemy enemy in _currentStageEnemies)
            {
                enemy.OnDeadEvent.AddListener(HandleEnemyDead);
            }
        }

        private void HandleEnemyDead(Entity entity)
        {
            Debug.Log("적 죽음");
            _currentStageEnemies.Remove(entity as Enemy);

            if (_currentStageEnemies.Count == 0)
            {
                combatChannel.RaiseEvent(CombatEvents.CombatEndEvent);
            }
        }
    }
}