using System.Collections.Generic;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI.InGame.MainHud
{
    public class SoulView : ViewBase
    {
        [SerializeField] private SoulIcons soulIcons;
        [SerializeField] private SkillCoolTimeList skillItem;

        protected override List<IUIBindable> InitializeBindables()
        {
            var list = base.InitializeBindables();
            list.Add(soulIcons);
            list.Add(skillItem);
            return list;
        }
    }
}