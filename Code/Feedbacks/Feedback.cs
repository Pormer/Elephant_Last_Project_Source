using Feedbacks.Contexts;
using UnityEngine;

namespace Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void PlayFeedback();
        public abstract void StopFeedback();
        
        private void OnDisable()
        {
            StopFeedback();
        }
    }

    public abstract class Feedback<T> : Feedback where T : FeedbackContext
    {
        protected T _context;

        public virtual void Setup(T context)
        {
            _context = context;
        }   
    }
}