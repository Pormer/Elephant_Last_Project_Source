using EventSystem;
using Feedbacks.Contexts;
using UnityEngine;

namespace Feedbacks
{
    public class ImpulseFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private ImpulseFeedbackDataSO impulseData;
        

        public override void PlayFeedback()
        {
            var evt = FeedbackEvents.ImpulseFeedbackEvent.Initializer(impulseData);
            feedbackChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
        }
    }
}