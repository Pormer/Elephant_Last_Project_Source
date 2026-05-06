using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class RectTransformStyle : BaseStyle<RectTransform, RectTransformStyleData>
    {
        [SerializeField] private bool doesUseSize;
        [SerializeField] private bool doesUsePosition;

        private Tweener _deltaTweener;

        protected override async UniTask ApplyStyle(RectTransformStyleData currentState)
        {
            if (_isInitialized)
            {
                _deltaTweener?.Kill();

                if (doesUseSize)
                    _deltaTweener = target.DOSizeDelta(currentState.SizeDelta, currentState.Duration)
                        .SetEase(currentState.Ease)
                        .SetUpdate(true);
                if (doesUsePosition)
                    _deltaTweener = target.DOAnchorPos(currentState.Position, currentState.Duration)
                        .SetEase(currentState.Ease)
                        .SetUpdate(true);

                await UniTask.WaitForSeconds(currentState.Duration, true);
            }
            else
            {
                if (doesUseSize)
                    target.sizeDelta = currentState.SizeDelta;
                if (doesUsePosition)
                    target.anchoredPosition = currentState.Position;
            }
        }
    }

    [Serializable]
    public class RectTransformStyleData : BaseStyleData
    {
        public Vector2 SizeDelta;
        public Vector2 Position;
    }
}