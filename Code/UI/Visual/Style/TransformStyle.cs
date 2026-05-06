using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public class TransformStyle : BaseStyle<Transform, TransformStyleData>
    {
        [SerializeField] private bool useScale = true;
        [SerializeField] private bool useRotation = true;
        private Tweener _scaleTweener;
        private Tweener _rotateTweener;

        protected override async UniTask ApplyStyle(TransformStyleData currentState)
        {
            if (_isInitialized)
            {
                if (useScale)
                    _scaleTweener?.Kill();
                if (useRotation)
                    _rotateTweener?.Kill();

                var targetScaleVec3 = new Vector3(currentState.Scale.x, currentState.Scale.y, 1);

                if (useScale)
                    _scaleTweener = target.DOScale(targetScaleVec3, currentState.Duration).SetEase(currentState.Ease)
                        .SetUpdate(true);
                if (useRotation)
                    _rotateTweener = target.DORotate(currentState.Rotation, currentState.Duration)
                        .SetEase(currentState.Ease).SetUpdate(true);

                await UniTask.WaitForSeconds(currentState.Duration, true);
            }
            else
            {
                if (useScale)
                    target.localScale = new Vector3(currentState.Scale.x, currentState.Scale.y, 1);
                if (useRotation)
                    target.rotation = Quaternion.Euler(currentState.Rotation);
            }
        }
    }

    [Serializable]
    public class TransformStyleData : BaseStyleData
    {
        public Vector2 Scale = Vector2.one;
        public Vector3 Rotation = Vector3.zero;
    }
}