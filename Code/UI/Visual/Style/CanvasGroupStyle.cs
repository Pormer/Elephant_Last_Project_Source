using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class CanvasGroupStyle : BaseStyle<CanvasGroup, CanvasGroupStyleData>
    {
        private Tweener _alphaTweener;

        protected override async UniTask ApplyStyle(CanvasGroupStyleData currentState)
        {
            if (target == null) return;

            _alphaTweener?.Kill();
            if (_isInitialized && currentState.Duration > 0f)
            {
                _alphaTweener = DOVirtual.Float(target.alpha, currentState.Alpha, currentState.Duration, value => target.alpha = value)
                    .SetEase(currentState.Ease)
                    .SetUpdate(true);
                await _alphaTweener.AsyncWaitForCompletion().AsUniTask();
            }

            target.alpha = currentState.Alpha;
        }
    }

    [Serializable]
    public class CanvasGroupStyleData : BaseStyleData
    {
        public float Alpha = 1;
    }
}
