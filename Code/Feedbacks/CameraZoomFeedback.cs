using DG.Tweening;
using EventSystem;
using UnityEngine;

namespace Feedbacks
{
    public class CameraZoomFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;

        [SerializeField] private float addZoomValue = -10.0f;
        [SerializeField] private float duration = 1f;
        [SerializeField] private Ease easeType;

        public override void PlayFeedback()
        {
            var evt = 
                FeedbackEvents.CameraZoomFeedbackEvent.Initializer(addZoomValue, duration, easeType);
            feedbackChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
        }
    }
}