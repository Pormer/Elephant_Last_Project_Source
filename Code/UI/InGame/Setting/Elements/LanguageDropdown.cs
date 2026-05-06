using System;
using Code.Setting;
using Code.UI.InGame.CustomElements;

namespace Code.UI.InGame.Setting.Elements
{

    public class LanguageDropdown : CustomDropdown<LanguageType>, ISettingElement
    {
        public Action<bool> OnSubElementFocus { get; set; }
        public override void HandleExpand(bool expanded)
        {
            base.HandleExpand(expanded);
            OnSubElementFocus?.Invoke(expanded);
        }
    }
}