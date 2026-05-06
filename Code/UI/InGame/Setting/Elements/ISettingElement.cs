using System;
using UnityEngine;

namespace Code.UI.InGame.Setting.Elements
{
    public interface ISettingElement
    {
        public Action<bool> OnSubElementFocus{get;set;}
    }
}