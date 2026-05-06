using System;
using Code.Entities.StatSystem;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI.InGame.Menu.StatUpgrade
{
    public class UpgradeDesc : ItemDesc, IUIBindable
    {
        public Enum BindKey => StatUIProperty.FocusUpgradeData;

        //임시
        private const string TitleFormat = "{0} {1} 증가";

        public void Bind(object v)
        {
            if (v is not StatUpgradeDataSO data) return;
            var targetStat = data.TargetStat;
            SetInfo(targetStat.Icon, string.Format(TitleFormat, targetStat.DisplayName, data.UpgradeValue),
                targetStat.description);
        }
    }
}