using System;
using System.Collections.Generic;
using System.Linq;
using Code.Entities.StatSystem;
using Code.Players;
using Cysharp.Threading.Tasks;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.UI.InGame.Models
{
    public enum StatUIProperty
    {
        UpgradeDataList,
        LockedDataList,
        FocusUpgradeData,
    }

    public class StatModel : ModelBase
    {
        [SerializeField] private StatUpgradeListSO statUpgradeList;
        [Inject] private Player _player;
        private EntityStatUpgradeManagement _management;

        public List<List<StatUpgradeDataSO>> UpgradeDataList
        {
            get => GetProperty<List<List<StatUpgradeDataSO>>>(StatUIProperty.UpgradeDataList);
            set => SetProperty(StatUIProperty.UpgradeDataList, value);
        }

        public List<StatUpgradeDataSO> LockedDataList
        {
            get => GetProperty<List<StatUpgradeDataSO>>(StatUIProperty.LockedDataList);
            set => SetProperty(StatUIProperty.LockedDataList, value);
        }

        public StatUpgradeDataSO FocusedUpgradeData
        {
            get => GetProperty<StatUpgradeDataSO>(StatUIProperty.FocusUpgradeData);
            set => SetProperty(StatUIProperty.FocusUpgradeData, value);
        }

        public Action<StatUpgradeDataSO> OnUpgrade;

        protected override async void Initialize()
        {
            base.Initialize();
            UpgradeDataList = SortUpgradeList(statUpgradeList.upgradeDataList);
            FocusedUpgradeData = null;
            UpdateLockedData();
            await UniTask.WaitForEndOfFrame();
            if (_player != null)
                _management = _player.GetModule<EntityStatUpgradeManagement>();
        }

        public bool CanUpgradeStat(StatUpgradeDataSO upgradeData)
        {
            return _management != null && _management.CanUpgradeStat(upgradeData);
        }

        public void UpgradeStat(StatUpgradeDataSO upgradeData)
        {
            if (_management == null)
            {
                Debug.LogError($"[Stat Model] Stat Management is null!!!");
            }

            _management.UpgradeStat(upgradeData);
            UpdateLockedData();
            OnUpgrade?.Invoke(upgradeData);
        }

        public void UpdateLockedData()
        {
            if (LockedDataList == null || LockedDataList.Count == 0)
                LockedDataList = statUpgradeList.upgradeDataList.FindAll(data => !CanUpgradeStat(data));
            else
                LockedDataList = LockedDataList.FindAll(data => !CanUpgradeStat(data));
        }

        private static List<List<StatUpgradeDataSO>> SortUpgradeList(List<StatUpgradeDataSO> upgradeDataList)
        {
            var result = new List<List<StatUpgradeDataSO>>();
            if (upgradeDataList == null || upgradeDataList.Count == 0)
            {
                return result;
            }

            var sourceSet = new HashSet<StatUpgradeDataSO>(upgradeDataList);
            var computedLevels = new Dictionary<StatUpgradeDataSO, int>();
            var visiting = new HashSet<StatUpgradeDataSO>();

            foreach (var data in upgradeDataList)
            {
                var level = ComputeLevel(data, sourceSet, computedLevels, visiting);
                while (result.Count <= level)
                {
                    result.Add(new List<StatUpgradeDataSO>());
                }

                result[level].Add(data);
            }

            return result;
        }

        private static int ComputeLevel(
            StatUpgradeDataSO data,
            HashSet<StatUpgradeDataSO> sourceSet,
            Dictionary<StatUpgradeDataSO, int> computedLevels,
            HashSet<StatUpgradeDataSO> visiting)
        {
            if (data == null) return 0;

            if (computedLevels.TryGetValue(data, out var cachedLevel)) return cachedLevel;

            if (!visiting.Add(data)) return 0;

            var maxDependencyLevel = -1;
            var dependencies = data.needStatUpgradeList;
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency == null || !sourceSet.Contains(dependency))
                    {
                        continue;
                    }

                    var dependencyLevel = ComputeLevel(dependency, sourceSet, computedLevels, visiting);
                    if (dependencyLevel > maxDependencyLevel)
                    {
                        maxDependencyLevel = dependencyLevel;
                    }
                }
            }

            visiting.Remove(data);

            var level = maxDependencyLevel + 1;
            computedLevels[data] = level;
            return level;
        }
    }
}