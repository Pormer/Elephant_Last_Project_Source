using Code.UI.InputBind;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = Code.Setting.DeviceType;

namespace Code.UI.InGame
{
    public class KeyIcon : MonoBehaviour
    {
        [SerializeField] private KeyIconDataSO iconData;
        [SerializeField] private Image icon;

        public void SetIcon(string key, DeviceType deviceType = DeviceType.None)
        {
            if (deviceType == DeviceType.None)
                icon.sprite = iconData.GetIcon(key);
            else
                icon.sprite = iconData.GetIcon(key, deviceType);
        }
    }
}