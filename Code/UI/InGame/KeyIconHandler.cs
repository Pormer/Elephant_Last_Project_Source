using UnityEngine;

namespace Code.UI.InGame
{
    [RequireComponent(typeof(KeyIcon))]
    public class KeyIconHandler : MonoBehaviour
    {
        [SerializeField] private string keyName;
        [SerializeField] private KeyIcon keyIcon;

        private void Awake()
        {
            keyIcon.SetIcon(keyName);
        }
    }
}