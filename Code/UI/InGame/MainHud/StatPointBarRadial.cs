using System;
using Code.UI.InGame.Models;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.MainHud
{
    public class StatPointBarRadial : StatPointBar
    {
        [SerializeField] protected Image mainSlider;
        [SerializeField] protected Image subSlider;
        [SerializeField] private float maxFill = 0.5f;
        private int _currentValue;
        private int _maxValue;


        public override void SetMaxValue(int value)
        {
            _maxValue = value;
            mainSlider.fillAmount = (float)value / _maxValue * maxFill;
        }

        protected override void SetMainSliderValue(int value)
        {
            Debug.Log($"Setting {BindKey}  to {value} / {_maxValue}");
            var target =  (float)value / _maxValue *  maxFill;
            mainSlider.fillAmount = target;
        }

        protected override Tweener SetSubSliderValue(int value)
        {
            float prev = subSlider.fillAmount;
            var target = (float)value / _maxValue * maxFill;
            return DOVirtual.Float(prev, target, duration, e => subSlider.fillAmount = e);
        }

        protected override void SetMainSliderColor(Color color)=>mainSlider.color = color;
        protected override void SetSubSliderColor(Color color)=>subSlider.color = color;
    }
}