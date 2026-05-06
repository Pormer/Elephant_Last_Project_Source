using System;
using Code.Souls.Core;
using Code.UI.InGame.InputList;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class EquipSoulItem : MonoBehaviour
    {
        [SerializeField] private SoulType soulType;
        [SerializeField] private GameObject emptyImage;
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private UIViewElement viewElement;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image gradientImage;
        [SerializeField] private Image outlineImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private SkillList skillList;
        [SerializeField] private InputMap inputMap;

        private float _gradientAlpha;

        private const string EmptyKey = "empty";
        private const string EquipKey = "equip";

        public SoulType SoulType => soulType;
        public SoulDataSO TargetData { get; private set; }

        public Action<SoulDataSO, SoulType> OnFocus;
        public Action OnPressed;
        public PointerHandler PointerHandler => pointerHandler;

        private void Awake()
        {
            _gradientAlpha = gradientImage.color.a;
            pointerHandler.Pressed += HandlePressed;
            pointerHandler.Focused += HandleFocus;
        }

        private void OnDestroy()
        {
            pointerHandler.Pressed -= HandlePressed;
            pointerHandler.Focused -= HandleFocus;
        }

        private void HandlePressed() => OnPressed?.Invoke();
        private void HandleFocus() => OnFocus?.Invoke(TargetData, soulType);

        public void SetInfo(SoulDataSO target)
        {
            TargetData = target;
            if (!target)
            {
                SetEmptyInfo();
                inputMap.SetKey();
                return;
            }

            inputMap.SetKey(EquipKey);
            emptyImage.SetActive(false);
            iconImage.gameObject.SetActive(true);
            skillList.gameObject.SetActive(true);

            var gradientColor = target.soulColor;
            gradientColor.a = _gradientAlpha;
            gradientImage.color = gradientColor;
            outlineImage.color = target.soulColor;
            nameText.color = target.soulColor;

            viewElement.RemoveState(EmptyKey).Forget();
            iconImage.sprite = target.Icon;
            nameText.text = target.DisplayName;
            skillList.SetSoul(target);
            ShowEffect();
        }

        private async void ShowEffect()
        {
            await viewElement.AddState(EquipKey);
            await viewElement.RemoveState(EquipKey);
        }

        private void SetEmptyInfo()
        {
            emptyImage.SetActive(true);
            iconImage.gameObject.SetActive(false);
            skillList.gameObject.SetActive(false);
            viewElement.AddState(EmptyKey).Forget();
            nameText.text = "";
        }
    }
}