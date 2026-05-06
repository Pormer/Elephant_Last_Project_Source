using System;
using Code.Souls.Core;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class SoulItem : MonoBehaviour
    {
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private Image icon;
        [SerializeField] private Image gradient;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI subNameText;
        [SerializeField] private UIViewElement equippedElement;
        [SerializeField] private GameObject newItem;

        private float gradientAlpha;

        private const string EquipKey = "equip";

        public PointerHandler PointerHandler => pointerHandler;
        public SoulDataSO TargetData { get; private set; }

        public Action<SoulDataSO> Focused;

        public void Initialize()
        {
            pointerHandler.Focused += HandleFocused;
            gradientAlpha = gradient.color.a;
        }

        private void OnDestroy()
        {
            pointerHandler.Focused -= HandleFocused;
        }

        private void HandleFocused()
        {
            Focused?.Invoke(TargetData);
            SetNew(false);
        }

        public void SetInfo(SoulDataSO target)
        {
            TargetData = target;
            icon.sprite = target == null ? null : target.Icon;
            var soulColor = target == null ? Color.clear : target.soulColor;
            nameText.text = target == null ? "" : target.DisplayName;
            nameText.color = soulColor;
            soulColor.a = gradientAlpha;
            gradient.color = soulColor;
            //?�직 ?�음 :3
            // subNameText.text = target.DisplayName;
        }

        public void SetEquipped(bool equipped)
        {
            if (equipped)
                equippedElement.AddState(EquipKey, 0).Forget();
            else
                equippedElement.RemoveState(EquipKey).Forget();
        }

        public void SetNew(bool isNew)
        {
            newItem.SetActive(isNew);
        }
    }
}