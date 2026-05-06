using Code.Entities.StatSystem;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame.Menu.StatUpgrade
{
    public class UpgradeHolder : ItemHolder<StatUpgradeDataSO>
    {
        [SerializeField] private UIViewElement element;
        [SerializeField] private TextMeshProUGUI upgradeText;

        //임시!!!!!!!!!!!!!!
        private int _upgradeCount = 0;

        private const string UpgradeFormat = "{0}/{1}";
        private const string LockKey = "lock";

        protected override Sprite GetIcon(StatUpgradeDataSO data) => data.TargetStat.Icon;

        public override void Initialize(StatUpgradeDataSO data)
        {
            base.Initialize(data);
            pointerHandler.interactable = data != null;
            UpdateText();
        }

        public void SetLock(bool locked)
        {
            Debug.Log($"Set lock  to {locked}");
            if (locked)
                element.AddState(LockKey, 10).Forget();
            else
                element.RemoveState(LockKey).Forget();
            pointerHandler.CanBePressed = locked;
        }

        public void Upgrade()
        {
            _upgradeCount++;
            UpdateText();
        }

        private void UpdateText() => upgradeText.text = string.Format(UpgradeFormat, _upgradeCount, TargetData.MaxUpgradeCount);
    }
}