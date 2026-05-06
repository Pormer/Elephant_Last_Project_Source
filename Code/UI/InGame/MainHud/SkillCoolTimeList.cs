using System;
using System.Collections.Generic;
using Code.Skills;
using Code.Skills.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI.InGame.MainHud
{
    public class SkillCoolTimeList : MonoBehaviour, IUIBindable
    {
        [SerializeField] private SkillCoolTimeIcon[] skillCoolTimeIcons;

        public Enum BindKey => SoulUIProperty.CurrentSkills;

        public void Bind(object v)
        {
            if (v is not List<Skill> value || value.Count == 0)
            {
                SetSkillLActive(false);
                return;
            }
            SetSkillLActive(true);
            
            for (int i = 0; i < value.Count; i++)
                skillCoolTimeIcons[i].SetSkill(value[i]);
        }

        private void SetSkillLActive(bool v)
        {
            foreach (var icon in skillCoolTimeIcons)
            {
                icon.gameObject.SetActive(v);
            }
        }
    }
}