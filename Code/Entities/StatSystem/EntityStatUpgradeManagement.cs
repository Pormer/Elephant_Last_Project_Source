using System.Collections.Generic;
using System.Linq;
using Code.Modules;
using UnityEngine;

namespace Code.Entities.StatSystem
{
    public class EntityStatUpgradeManagement : MonoBehaviour, IModule
    {
        [SerializeField] private StatUpgradeListSO upgradeList;
        
        private EntityStatCompo _statCompo;
        private Dictionary<StatUpgradeDataSO, int> _upgradeCountDict;
        
        public void Initialize(ModuleOwner owner)
        {
            _upgradeCountDict = upgradeList.upgradeDataList.ToDictionary(data => data, data => 0);
            
            _statCompo = owner.GetModule<EntityStatCompo>();
            Debug.Assert(_statCompo != null, "stat compo is null");
        }

        public bool CanUpgradeStat(StatUpgradeDataSO upgradeData)
        {
            foreach (var needData in upgradeData.needStatUpgradeList)
            {
                if (!_upgradeCountDict.TryGetValue(needData, out var count) || count <= 0)
                    return false;
            }

            return _upgradeCountDict[upgradeData] < upgradeData.MaxUpgradeCount;
        }

        public void UpgradeStat(StatUpgradeDataSO upgradeData)
        {
            _upgradeCountDict.TryAdd(upgradeData, 0);
            _upgradeCountDict[upgradeData]++;
            
            var targetStat = _statCompo.GetStat(upgradeData.TargetStat);
            if (targetStat == null) return;
            
            _statCompo.AddModifier(targetStat, upgradeData, upgradeData.UpgradeValue);
        }

        public void ClearStatUpgrade(StatUpgradeDataSO upgradeData)
        {
            _upgradeCountDict.Remove(upgradeData);
            _statCompo.RemoveModifier(upgradeData.TargetStat, upgradeData);
        }
    }
}