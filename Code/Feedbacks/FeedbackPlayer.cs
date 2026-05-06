using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feedbacks
{
    public class FeedbackPlayer : MonoBehaviour
    {
        private List<Feedback> _feedbackToPlay; 

        private void Awake()
        {
            _feedbackToPlay = GetComponents<Feedback>().ToList();
        }

        public void PlayFeedbacks()
        {
            StopFeedbacks();
            _feedbackToPlay.ForEach(f => f.PlayFeedback());
        }

        public void StopFeedbacks()
        {
            _feedbackToPlay.ForEach(f => f.StopFeedback());
        }
    }
}