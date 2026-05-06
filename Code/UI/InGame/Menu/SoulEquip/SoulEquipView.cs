using System;
using System.Collections.Generic;
using Code.Skills.Core;
using Code.Souls.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Code.UI.UIStateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public enum SoulEquipSection
    {
        None = -1,
        EquippedSoul,
        SoulList,
        SoulInfo
    }

    public class SoulEquipView : ViewBase, ISubUIState, ISelectableState
    {
        [Header("UI Styles")] [SerializeField] private Canvas canvas;
        [SerializeField] private UIViewElement canvasElement;
        [Header("Sections")] [SerializeField] private EquipSoulList equipSoulList;
        [SerializeField] private SoulListSection soulListSection;
        [SerializeField] private SoulInfoSection soulInfoSection;
        [SerializeField] private SkillDesc skillDesc;

        [Header("References")] [SerializeField]
        private UIInputSO inputSO;

        public UIStateType StateType => UIStateType.SoulEquip;
        public GameObject GameObject => gameObject;
        public Selectable DefaultFocusable => equipSoulList.FirstSelectable;

        private Dictionary<SoulEquipSection, UISection> _sections;
        private UISection _currentSection;

        private SoulEquipSection _currentSectionType = SoulEquipSection.None;
        public SoulEquipSection CurrentSectionType => _currentSectionType;
        private SoulEquipSection _previousSectionType = SoulEquipSection.None;
        public SoulEquipSection PreviousSectionType => _previousSectionType;

        private bool _isChanging = false;

        private const string HideKey = "hide";

        public delegate void SoulEquipCancel(SoulEquipSection currentSection, SoulEquipSection previousSection);

        public SoulEquipCancel OnCancelPressed;
        public Action OnInfoPressed;
        public Action OnItemPressed;
        public Action OnEquipPressed;
        public Action<SoulDataSO> OnItemFocused;
        public Action<SkillDataSO> OnSkillFocus;

        private void Awake()
        {
            InitializeSections();
            SubscribeSectionEvents();
            DisableAllSections();
        }

        private void OnDestroy()
        {
            UnsubscribeSectionEvents();
        }

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await canvasElement.RemoveState(HideKey);
            InitializeSectionViews();
            SubscribeInputEvents();
            await SetSectionAsync(SoulEquipSection.EquippedSoul);
        }

        public async UniTask OnExit()
        {
            DisableAllSections();
            UnsubscribeInputEvents();
            await canvasElement.AddState(HideKey);
            canvas.enabled = false;
        }

        private void HandleItemFocused(SoulDataSO soulData) => OnItemFocused?.Invoke(soulData);

        private void HandleItemPressed()
        {
            if (_currentSection == equipSoulList) return;
            OnItemPressed?.Invoke();
        }

        private void HandleSkillFocus(SkillDataSO skillData) => OnSkillFocus?.Invoke(skillData);

        private void ClearSkillFocus()
        {
            if (_currentSection == soulInfoSection)
                OnSkillFocus?.Invoke(null);
        }

        private void HandleCancelPressed() => OnCancelPressed?.Invoke(_currentSectionType, _previousSectionType);

        private void HandleInfoPressed()
        {
            if (_isChanging) return;
            OnInfoPressed?.Invoke();
        }

        protected override List<IUIBindable> InitializeBindables()
        {
            return new List<IUIBindable>
            {
                equipSoulList,
                soulListSection,
                soulInfoSection,
                skillDesc
            };
        }

        protected override void HandleBinding(Enum key, object value)
        {
            base.HandleBinding(key, value);
            if (Equals(key, SoulUIProperty.CurrentSoulSlot) && value is Dictionary<SoulType, SoulDataSO> v)
            {
                if (v.TryGetValue(SoulType.God, out var g))
                    soulListSection.SetEquipped(SoulType.God, g);
                if (v.TryGetValue(SoulType.Devil, out var d))
                    soulListSection.SetEquipped(SoulType.Devil, d);
            }
        }

        public void SetSection(SoulEquipSection section)
        {
            SetSectionAsync(section).Forget();
        }

        public async UniTask SetSectionAsync(SoulEquipSection section)
        {
            if (_isChanging) return;
            if (!_sections.TryGetValue(section, out var nextSection)) return;

            _isChanging = true;
            try
            {
                DeactivateCurrentSection();
                await nextSection.SetFocus(true);
                _currentSection = nextSection;
                if (_currentSectionType != section)
                {
                    _previousSectionType = _currentSectionType;
                    _currentSectionType = section;
                }
            }
            finally
            {
                _isChanging = false;
            }
        }

        private void DisableAllSections()
        {
            ClearSkillFocus();
            _currentSection = null;

            foreach (var section in _sections.Values)
                section?.SetFocus(false);
        }

        private void InitializeSections()
        {
            _sections ??= new Dictionary<SoulEquipSection, UISection>
            {
                { SoulEquipSection.EquippedSoul, equipSoulList },
                { SoulEquipSection.SoulList, soulListSection },
                { SoulEquipSection.SoulInfo, soulInfoSection }
            };
        }

        private void SubscribeSectionEvents()
        {
            equipSoulList.OnPressed += HandleEquipPressed;
            equipSoulList.OnFocus += HandleEquipFocused;
            soulListSection.OnItemFocused += HandleItemFocused;
            soulInfoSection.OnSkillFocus += HandleSkillFocus;
        }

        private void HandleEquipFocused(SoulDataSO obj, SoulType soulType)
        {
            soulListSection.SetCurrentSoul(soulType);
            OnItemFocused?.Invoke(obj);
        }

        private void HandleEquipPressed()
        {
            OnEquipPressed?.Invoke();
        }

        private void UnsubscribeSectionEvents()
        {
            equipSoulList.OnPressed -= HandleEquipPressed;
            equipSoulList.OnFocus -= HandleEquipFocused;
            soulListSection.OnItemFocused -= HandleItemFocused;
            soulInfoSection.OnSkillFocus -= HandleSkillFocus;
        }

        private void DeactivateCurrentSection()
        {
            if (_currentSection == null) return;

            ClearSkillFocus();
            _currentSection.SetFocus(false).Forget();
        }

        private void InitializeSectionViews()
        {
            foreach (var section in _sections.Values)
                section.InitializeView();
        }

        private void SubscribeInputEvents()
        {
            inputSO.OnInfoPressed += HandleInfoPressed;
            inputSO.OnCancelPressed += HandleCancelPressed;
            inputSO.OnPausePressed += HandleCancelPressed;
            inputSO.OnSubmitPressed += HandleItemPressed;
        }

        private void UnsubscribeInputEvents()
        {
            inputSO.OnInfoPressed -= HandleInfoPressed;
            inputSO.OnCancelPressed -= HandleCancelPressed;
            inputSO.OnPausePressed -= HandleCancelPressed;
            inputSO.OnSubmitPressed -= HandleItemPressed;
        }
    }
}