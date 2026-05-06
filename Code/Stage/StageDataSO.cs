using System.Collections.Generic;
using Code.Enemies.Core;
using UnityEngine;

namespace Code.Stage
{
    [CreateAssetMenu(fileName = "StageDataSO", menuName = "SO/Stage/StageDataSO", order = 0)]
    public class StageDataSO : ScriptableObject
    {
        public List<EnemySpawnData> spawnDataList;
    }
}