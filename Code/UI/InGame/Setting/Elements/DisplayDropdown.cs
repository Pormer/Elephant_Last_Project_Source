using System;
using Code.Setting;
using Code.UI.InGame.CustomElements;

namespace Code.UI.InGame.Setting.Elements
{
    public class DisplayDropdown : CustomDropdown<ResolutionType>, ISettingElement
    {
        public Action<bool> OnSubElementFocus { get; set; }
        public override void HandleExpand(bool expanded)
        {
            base.HandleExpand(expanded);
            OnSubElementFocus?.Invoke(expanded);
        }
    }
}