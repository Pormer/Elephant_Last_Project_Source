using System;
using System.Collections.Generic;
using System.Linq;
using Code.Skills.Core;
using Code.Souls.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class SoulInfoSection : UISection, IUIBindable
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI subnameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private RectTransform descPart;
        [SerializeField] private SkillList skillList;
        [SerializeField] private SkillDesc skillDesc;

        private List<SkillItem> _skillItems = new();
        private Dictionary<PointerHandler, SkillItem> _pointerHandlers = new();

        public override Selectable FirstSelectable => _pointerHandlers.Keys.FirstOrDefault();

        public Action<SkillDataSO> OnSkillFocus;

        public Enum BindKey => SoulUIProperty.FocusedSoul;
        public override void InitializeView() { }

        public void Bind(object v)
        {
            if (v is not SoulDataSO skillData || skillData == null)
            {
                SetEmpty();
                return;
            }

            SetInfo(skillData);
        }

        public void SetInfo(SoulDataSO info)
        {
            if (info == null)
            {
                Debug.Log("info is null");
                SetEmpty();
                return;
            }

            SetInfoVisible(true);
            ApplySoulText(info);
            skillList.SetSoul(info);
            RebuildSkillPointerHandlers();
            BindSkillFocusEvents();
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.MarkLayoutForRebuild(descPart);
        }

        private void RebuildSkillPointerHandlers()
        {
            _skillItems = skillList.SkillItems;
            ClearSkillFocusHandlers();
            _pointerHandlers.Clear();

            foreach (var skillItem in _skillItems)
            {
                if (skillItem == null) continue;

                var pointerHandler = skillItem.GetComponent<PointerHandler>();
                if (pointerHandler == null) continue;

                _pointerHandlers[pointerHandler] = skillItem;
            }
        }

        private void SetEmpty()
        {
            SetInfoVisible(false);
            icon.sprite = null;
            nameText.text = string.Empty;
            subnameText.text = string.Empty;
            descriptionText.text = string.Empty;
            ClearSkillFocusHandlers();
            _pointerHandlers.Clear();
        }

        public override async UniTask SetFocus(bool value)
        {
            await base.SetFocus(value);
            if (!value)
                OnSkillFocus?.Invoke(null);
            skillDesc.gameObject.SetActive(value);
        }

        private void SetInfoVisible(bool isVisible)
        {
            icon.gameObject.SetActive(isVisible);
            descPart.gameObject.SetActive(isVisible);
            skillList.gameObject.SetActive(isVisible);
        }

        private void ApplySoulText(SoulDataSO info)
        {
            icon.sprite = info.Icon;
            nameText.text = info.DisplayName;
            descriptionText.text = info.Description;
        }

        private void BindSkillFocusEvents()
        {
            foreach (var pair in _pointerHandlers)
                pair.Key.Focused = () => OnSkillFocus?.Invoke(pair.Value.SkillData);
        }

        private void ClearSkillFocusHandlers()
        {
            foreach (var pair in _pointerHandlers)
            {
                if (pair.Key != null)
                    pair.Key.Focused = null;
            }
        }
    }
}
