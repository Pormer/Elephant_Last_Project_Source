using EventSystem;
using UnityEngine;

namespace Feedbacks
{
    public class UseDefaultBGMFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        
        public override void PlayFeedback()
        {
            feedbackChannel.RaiseEvent(FeedbackEvents.UseDefaultBGMEvent);
        }

        public override void StopFeedback()
        {
            
        }
    }
}