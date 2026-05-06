using Code.UI.InputBind;
using UnityEngine;

namespace Code.UI.InGame
{
    public class PlayerKeyIcon : KeyIcon
    {
        [SerializeField] private string key;
        [SerializeField] private int idx = -1;
        [SerializeField] private UIInputBindSO keyBind;
        [SerializeField] private bool isPlayer = true;

        private string _key;

        private void OnEnable()
        {
            if (isPlayer)
                _key = keyBind.GetPlayerKey(key, idx);
            else
                _key = keyBind.GetUIKey(key, idx);
            SetIcon(_key);
        }

        private void Awake()
        {
            keyBind.OnKeyChanged += HandleKeyChange;
        }

        private void OnDestroy()
        {
            keyBind.OnKeyChanged -= HandleKeyChange;
        }

        private void HandleKeyChange()
        {
            if (isPlayer)
                _key = keyBind.GetPlayerKey(key, idx);
            else
                _key = keyBind.GetUIKey(key, idx);
            SetIcon(_key);
        }
    }
}