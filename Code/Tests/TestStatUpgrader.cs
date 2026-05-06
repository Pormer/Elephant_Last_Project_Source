using Code.Entities.StatSystem;
using Code.Modules;
using UnityEngine;

namespace Code.Tests
{
    public class TestStatUpgrader : MonoBehaviour
    {
        [SerializeField] private ModuleOwner target;
        [SerializeField] private StatUpgradeDataSO statUpgradeData;
        private EntityStatUpgradeManagement _statManagement;

        private void Start()
        {
            _statManagement = target.GetModule<EntityStatUpgradeManagement>();
        }

        [ContextMenu("Test Upgrade")]
        private void TestUpgrade()
        {
            if (!_statManagement.CanUpgradeStat(statUpgradeData))
            {
                Debug.Log("최대 레벨이거나 해금 되지 않은 스탯이여서 업그레이드 할 수 없습니다.");
                return;
            }
            
            _statManagement.UpgradeStat(statUpgradeData);
            Debug.Log($"{statUpgradeData.TargetStat.DisplayName} 스탯 업그레이드 성공.");
        }
    }
}