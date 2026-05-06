using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.StatSystem
{
    [CreateAssetMenu(fileName = "StatUpgradeData", menuName = "SO/Stat/Upgrade", order = 0)]
    public class StatUpgradeDataSO : ScriptableObject
    {
        [SerializeField] private StatSO targetStat;
        public StatSO TargetStat => targetStat;

        [SerializeField] private int maxUpgradeCount = 1;
        public int MaxUpgradeCount => maxUpgradeCount;
        
        [SerializeField] private float upgradeValue;
        public float UpgradeValue => upgradeValue;

        public List<StatUpgradeDataSO> needStatUpgradeList;
    }
}