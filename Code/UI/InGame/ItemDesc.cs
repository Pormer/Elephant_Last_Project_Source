using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class ItemDesc : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI namePlate;
        [SerializeField] private TextMeshProUGUI description;

        public void SetInfo(Sprite iconSprite, string displayName, string descriptionText)
        {
            if (iconSprite == null)
            {
                icon.gameObject.SetActive(false);
                icon.sprite = null;
            }
            else
            {
                icon.gameObject.SetActive(true);
                icon.sprite = iconSprite;
            }
            namePlate.text = displayName;
            description.text = descriptionText;
        }
    }
}