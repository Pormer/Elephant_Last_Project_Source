using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Visual.Style
{
    public class GraphicStyle : BaseStyle<Graphic, GraphicStyleData>
    {
        private Tweener _colorTweener;

        protected override async UniTask ApplyStyle(GraphicStyleData currentState)
        {
            if (target == null) return;

            _colorTweener?.Kill();
            if (_isInitialized && currentState.Duration > 0f)
            {
                _colorTweener = target.DOColor(currentState.Color, currentState.Duration)
                    .SetEase(currentState.Ease)
                    .SetUpdate(true);
                await _colorTweener.AsyncWaitForCompletion().AsUniTask();
            }
            target.color = currentState.Color;
        }
    }
    [Serializable]
    public class GraphicStyleData : BaseStyleData
    {
        public Color Color = Color.white;
    }
}
