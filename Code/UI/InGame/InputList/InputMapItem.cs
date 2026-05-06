using TMPro;
using UnityEngine;

namespace Code.UI.InGame.InputList
{
    public class InputMapItem : MonoBehaviour
    {
        [SerializeField] private KeyIcon keyIcon;
        [SerializeField] private TextMeshProUGUI infoName;
        private string _currentBindKey = string.Empty;
        private string _currentText = string.Empty;

        public bool SetBindKey(string key)
        {
            key ??= string.Empty;
            if (_currentBindKey == key) return false;
            _currentBindKey = key;
            keyIcon.SetIcon(key);
            return true;
        }
        
        public bool SetText(string text)
        {
            text ??= string.Empty;
            if (_currentText == text) return false;
            _currentText = text;
            infoName.text = text;
            return true;
        }
    }
}
