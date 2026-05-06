using UnityEngine;

namespace Code.UI.InGame.Setting.KeySetting
{
    public class PlayerBindItem : BindItem
    {
        [SerializeField] private string actionName;
        [SerializeField] private int index;

        protected override void Refresh()
        {
            if (BindSO == null)
                return;

            var key = BindSO.GetPlayerKey(actionName, DeviceType, index);
            SetIcon(key,DeviceType);
        }

        protected override void TriggerRebind()
        {
            BindSO?.StartPlayerRebind(actionName, DeviceType, index);
        }
    }
}
