using System.Collections;
using EventSystem;
using UnityEngine;

namespace Feedbacks
{
    public class TimeScaleFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;

        [SerializeField] private float changeTimeScale = 0.5f;
        [SerializeField] private float duration = 0.1f;

        public override void PlayFeedback()
        {
            var evt = 
                FeedbackEvents.TimeStopFeedbackEvent.Initializer(changeTimeScale, duration);
            feedbackChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
            Time.timeScale = 1;
        }
    }
}