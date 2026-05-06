using DG.Tweening;
using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class AlphaPanel : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private Image alphaImage;
        [SerializeField] private float duration;
        [SerializeField] [Range(0,1)] private float alpha;

        private void Awake()
        {
            uiChannel.AddListener<AlphaScreenEvent>(HandleSetAlpha);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<AlphaScreenEvent>(HandleSetAlpha);
        }
        
        private void HandleSetAlpha(AlphaScreenEvent evt)
        {
            Color currentColor = alphaImage.color;
            float alphaTemp = evt.isPop ? alpha : 0;
            alphaImage.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, alphaTemp), duration);
        }
    }
}