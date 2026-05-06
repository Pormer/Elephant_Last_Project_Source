using Ami.BroAudio;
using EventSystem;
using UnityEngine;

namespace Feedbacks
{
    public class BGMChangeFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private SoundID changeBgmId;
        
        public override void PlayFeedback()
        {
            var evt = FeedbackEvents.BGMChangeEvent.Initializer(changeBgmId);
            feedbackChannel.RaiseEvent(evt);
        }

        public override void StopFeedback()
        {
        }
    }
}