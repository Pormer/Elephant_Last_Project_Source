using System;
using UnityEngine;

namespace Code.Enemies.Core
{
    [Serializable]
    public struct EnemySpawnData
    {
        public EnemyDataSO enemyData;
        public Vector3 spawnPos;
        public EnemySpawnType spawnType;
    }
}