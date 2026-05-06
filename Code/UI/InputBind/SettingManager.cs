using Code.Setting;
using UnityEngine;

namespace Code.UI.InputBind
{
    [DefaultExecutionOrder(-1)]
    public class SettingManager : MonoBehaviour
    {
        [SerializeField] private UIInputBindSO  uiInputBindSO;
        [SerializeField] private UserSettingSO userSettingSo;
        [SerializeField] private KeyIconDataSO keyIconDataSO;

        private void Awake()
        {
            keyIconDataSO.Initialize();
            uiInputBindSO.Initialize();
            userSettingSo.Initialize();
        }

        private void OnDestroy()
        {
            userSettingSo.Dispose();
            uiInputBindSO.Dispose();
            keyIconDataSO.Dispose();
        }
    }
}