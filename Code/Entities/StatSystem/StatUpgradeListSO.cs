using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.StatSystem
{
    [CreateAssetMenu(fileName = "StatUpgradeList", menuName = "SO/Stat/UpgradeList", order = 0)]
    public class StatUpgradeListSO : ScriptableObject
    {
        public List<StatUpgradeDataSO> upgradeDataList;

        public StatUpgradeDataSO this[int idx] => upgradeDataList[idx];
        public int Count => upgradeDataList.Count;
    }
}