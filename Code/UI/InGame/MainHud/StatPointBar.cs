using System;
using Code.UI.InGame.Models;
using Code.UI.Interface;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.InGame.MainHud
{
    public abstract class StatPointBar : MonoBehaviour, IUIBindable
    {

        [SerializeField] protected Color mainColor;
        [SerializeField] protected Color subColor;
        [SerializeField] protected float delay = 0.125f;
        [SerializeField] protected float duration = 0.25f;
        [SerializeField] private PlayerUIProperty bindKey;
        public Enum BindKey => bindKey;
        private Tweener _subSliderTween;
        private int _valueVersion;
        
        public void Bind(object v)
        {
            if (v is not int value) return;
            SetValue(value);
        }

        public abstract void SetMaxValue(int value);

        protected async void SetValue(int value)
        {
            var version = ++_valueVersion;
            SetMainSliderValue(value);
            _subSliderTween?.Kill();
            await UniTask.WaitForSeconds(delay);
            if (version != _valueVersion) return;
            _subSliderTween = SetSubSliderValue(value);
        }

        protected abstract void SetMainSliderValue(int value);
        protected abstract Tweener SetSubSliderValue(int value);

        protected abstract void SetMainSliderColor(Color color);
        protected abstract void SetSubSliderColor(Color color);
#if UNITY_EDITOR
        private void OnValidate()
        {
            SetMainSliderColor(mainColor);
            SetSubSliderColor(subColor);
        }
#endif
    }
}