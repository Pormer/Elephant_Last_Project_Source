using EventSystem;
using UnityEngine;

namespace Code.Feedbacks.Managers
{
    public abstract class FeedbackManager : MonoBehaviour
    {
        [SerializeField] protected GameEventChannelSO feedbackChannel;
    }
}