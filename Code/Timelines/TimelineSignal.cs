using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Timelines
{
    public class TimelineSignal : MonoBehaviour
    {
        public UnityEvent OnStartTimelineEvent;
        
        [SerializeField] private GameEventChannelSO timelineChannel;
        [SerializeField] private TimelineType timelineType;
        
        public void SendPlayTimelineSignal()
        {
            var evt = TimelineEvents.PlayTimelineEvent.Initializer(timelineType);
            timelineChannel.RaiseEvent(evt);
        }
    }
}