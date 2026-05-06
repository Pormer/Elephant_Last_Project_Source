using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class TextObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshPro popupText;
        
        private Pool _pool;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool) => _pool = pool;

        public void ResetItem()
        {
            transform.localScale = Vector3.one;
            popupText.alpha = 1f;
        }
        
        public void ShowPopupText(string text, TextInfoSO textInfo, Vector3 position, float duration)
        {
            popupText.SetText(text);
            popupText.color = textInfo.textColor;
            popupText.fontSize = textInfo.fontSize;
            transform.position = position;

            float scaleTime = 0.2f;
            float fadeTime = 0.5f;
            
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(2.5f, scaleTime));
            seq.Append(transform.DOScale(1.2f, scaleTime));
            seq.AppendInterval(duration);
            seq.Append(transform.DOScale(0.3f, fadeTime));
            seq.Join(popupText.DOFade(0, fadeTime));
            seq.Join(transform.DOMoveY(position.y + 1f, fadeTime));
            seq.AppendCallback(() => _pool.Push(this));
        }
    }
}