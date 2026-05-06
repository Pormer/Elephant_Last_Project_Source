using System;
using Code.Entities.StatSystem;
using Code.UI.InGame.Models;
using UnityEngine;

namespace Code.UI.InGame.Menu.StatUpgrade
{
    public class StatUpgradePresenter : PresenterBase<StatModel, StatUpgradeView>
    {

        public override void Initialize(ModelBase modelBase)
        {
            base.Initialize(modelBase);

            view.UpgradeFocused += HandleUpgradeFocused;
            view.UpgradePressed += HandleUpgradePressed;
            view.CheckLocked += HandleCheckLocked;
            model.OnUpgrade += HandleUpgrade;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (view != null)
            {
                view.UpgradeFocused -= HandleUpgradeFocused;
                view.UpgradePressed -= HandleUpgradePressed;
                view.CheckLocked -= HandleCheckLocked;
                model.OnUpgrade -= HandleUpgrade;
            }
        }

        private void HandleCheckLocked()
        {
            model.UpdateLockedData();
        }

        private void HandleUpgrade(StatUpgradeDataSO obj)
        {
            view.UpgradeItem(obj);
        }

        private void HandleUpgradeFocused(StatUpgradeDataSO upgradeData)
        {
            if (model == null) return;
            model.FocusedUpgradeData = upgradeData;
        }

        private void HandleUpgradePressed(StatUpgradeDataSO upgradeData)
        {
            if (model == null || view == null || upgradeData == null) return;

            if (model.CanUpgradeStat(upgradeData))
            {
                model.UpgradeStat(upgradeData);
                model.FocusedUpgradeData = upgradeData;
            }
        }
    }
}
