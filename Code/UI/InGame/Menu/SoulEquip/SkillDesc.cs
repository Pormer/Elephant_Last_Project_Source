using System;
using Code.Skills;
using Code.Skills.Core;
using Code.UI.InGame.Models;
using Code.UI.Interface;

namespace Code.UI.InGame.Menu.SoulEquip
{
    public class SkillDesc : ItemDesc, IUIBindable
    {
        public Enum BindKey => SoulUIProperty.FocusedSkill;

        public void Bind(object v)
        {
            if(v is not SkillDataSO info)  return;
            if (info != null)
                SetInfo(info.Icon, info.DisplayName, info.Description);
            else
                SetInfo(null, string.Empty, string.Empty);
        }
    }
}
