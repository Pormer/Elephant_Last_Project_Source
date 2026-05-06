using System;
using System.Collections.Generic;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using UnityEngine;

namespace Code.UI.InGame.MainHud
{
    public class StatPointView : ViewBase
    {
        [SerializeField] private StatPointBar healthBar;
        [SerializeField] private StatPointBar[] manaBars;

        protected override List<IUIBindable> InitializeBindables()
        {
            var res = new List<IUIBindable>(manaBars);
            res.Add(healthBar);
            return res;
        }

        protected override void HandleBinding(Enum key, object value)
        {
            if (value is not int intValue) return;
            switch (key)
            {
                case PlayerUIProperty.PlayerMaxHealth:
                    healthBar.SetMaxValue(intValue);
                    break;
                case PlayerUIProperty.PlayerMaxMana:
                    foreach (var bar in manaBars) bar.SetMaxValue(intValue);
                    break;
            }
        }
    }
}