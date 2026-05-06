using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Visual.Style
{
    public class LayoutElementStyle : BaseStyle<LayoutElement, LayoutElementStyleData>
    {
        [SerializeField] private bool doesUsePreferredWidth;
        [SerializeField] private bool doesUsePreferredHeight;
        [SerializeField] private bool doesUseFlexibleWidth;
        [SerializeField] private bool doesUseFlexibleHeight;

        protected override async UniTask ApplyStyle(LayoutElementStyleData currentState)
        {
            if (_isInitialized)
            {
                var task = UniTask.CompletedTask;
                if (doesUsePreferredWidth)
                {
                    var pw = DOVirtual.Float(target.preferredWidth, currentState.PreferredWidth,
                        currentState.Duration,
                        v => target.preferredWidth = v).SetEase(currentState.Ease).SetUpdate(true);
                    task  = UniTask.WhenAll(task, pw.AsyncWaitForCompletion().AsUniTask());
                }

                if (doesUsePreferredHeight)
                {
                    var ph = DOVirtual.Float(target.preferredHeight, currentState.PreferredHeight,
                        currentState.Duration,
                        v => target.preferredHeight = v).SetEase(currentState.Ease).SetUpdate(true);
                    task  = UniTask.WhenAll(task, ph.AsyncWaitForCompletion().AsUniTask());
                }
                if (doesUseFlexibleWidth)
                {
                    var fw = DOVirtual.Float(target.flexibleWidth, currentState.FlexibleWidth,
                        currentState.Duration,
                        v => target.flexibleWidth = v).SetEase(currentState.Ease).SetUpdate(true);
                    task = UniTask.WhenAll(task, fw.AsyncWaitForCompletion().AsUniTask());
                }

                if (doesUseFlexibleHeight)
                {
                    var fh = DOVirtual.Float(target.flexibleHeight, currentState.FlexibleHeight,
                        currentState.Duration,
                        v => target.flexibleHeight = v).SetEase(currentState.Ease).SetUpdate(true);
                    task = UniTask.WhenAll(task, fh.AsyncWaitForCompletion().AsUniTask());
                }

                await task;
            }
            else
            {
                target.flexibleWidth = currentState.FlexibleWidth;
                target.flexibleHeight = currentState.FlexibleHeight;
            }
        }
    }

    [System.Serializable]
    public class LayoutElementStyleData : BaseStyleData
    {
        public float PreferredWidth;
        public float PreferredHeight;
        public float FlexibleWidth;
        public float FlexibleHeight;
    }
}