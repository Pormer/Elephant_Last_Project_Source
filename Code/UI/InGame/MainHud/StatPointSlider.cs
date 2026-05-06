using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.MainHud
{
    public class StatPointSlider : StatPointBar
    {
        [SerializeField] protected Slider mainSlider;
        [SerializeField] protected Slider subSlider;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI maxValueText;

        public override void SetMaxValue(int value)
        {
            mainSlider.maxValue = value;
            maxValueText.text = $"/{value}";
            mainSlider.value = value;

            subSlider.maxValue = value;
            subSlider.value = value;
        }

        protected override void SetMainSliderValue(int value)
        {
            mainSlider.value = value;
            valueText.text = $"{value}";
        }

        protected override Tweener SetSubSliderValue(int value)
        {
            var prev = subSlider.value;
            return DOVirtual.Float(prev, value, duration, e => subSlider.value = e);
        }

        protected override void SetMainSliderColor(Color color)
        {
            mainSlider.fillRect.GetComponent<Graphic>().color = color;
        }

        protected override void SetSubSliderColor(Color color)
        {
            subSlider.fillRect.GetComponent<Graphic>().color = color;
        }
    }
}