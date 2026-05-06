using System;
using Code.Skills.Core;
using Code.Souls.Core;
using Code.UI.InGame.Models;
using EventSystem;
using UnityEngine;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class SoulEquipPresenter : PresenterBase<SoulModel, SoulEquipView>
    {
        [SerializeField] private GameEventChannelSO uiChannelSO;

        public override void Initialize(ModelBase m)
        {
            base.Initialize(m);

            view.OnInfoPressed += HandleInfoPressed;
            view.OnItemFocused += HandleItemFocused;
            view.OnItemPressed += HandleItemPressed;
            view.OnCancelPressed += HandleCancelPressed;
            view.OnSkillFocus += HandleSkillFocus;
            view.OnEquipPressed += HandleEquipPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            view.OnInfoPressed -= HandleInfoPressed;
            view.OnItemFocused -= HandleItemFocused;
            view.OnItemPressed -= HandleItemPressed;
            view.OnCancelPressed -= HandleCancelPressed;
            view.OnSkillFocus -= HandleSkillFocus;
            view.OnEquipPressed -= HandleEquipPressed;
        }

        private void HandleEquipPressed()
        {
            view.SetSection(SoulEquipSection.SoulList);
        }


        private void HandleInfoPressed()
        {
            if (model.FocusedSoul != null)
                view.SetSection(SoulEquipSection.SoulInfo);
        }

        private void HandleItemFocused(SoulDataSO obj)
        {
            model.FocusedSoul = obj;
        }

        private void HandleItemPressed()
        {
            model.Equip(model.FocusedSoul);
            view.SetSection(SoulEquipSection.EquippedSoul);
        }

        private void HandleCancelPressed(SoulEquipSection currentSection, SoulEquipSection previousSection)
        {
            if(currentSection == SoulEquipSection.EquippedSoul)
                uiChannelSO.RaiseEvent(UIEvents.SubUICancelEvent);
            else
                view.SetSection(previousSection);
        }

        private void HandleSkillFocus(SkillDataSO obj) => model.FocusedSkill = obj;
    }
}
