using System;
using EventSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private PoolItemSO textItemData;
        [SerializeField] private TextInfoSO textInfoData;
        [SerializeField] private float duration;

        [SerializeField] private Vector3 spawnOffset;

        private void Awake()
        {
            feedbackChannel.AddListener<DamageTextFeedbackEvent>(HandleDamageTextFeedback);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<DamageTextFeedbackEvent>(HandleDamageTextFeedback);
        }
        
        private void HandleDamageTextFeedback(DamageTextFeedbackEvent evt)
        {
            ShowPopupText(evt.damage.ToString(), evt.position);
        }

        public void ShowPopupText(string text, Vector3 position)
        {
            TextObject textObject = PoolManagerMono.Instance.Pop<TextObject>(textItemData);
            textObject.ShowPopupText(text, textInfoData, position + spawnOffset, duration);
        }
    }
}