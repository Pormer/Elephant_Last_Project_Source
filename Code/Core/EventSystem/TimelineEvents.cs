using Code.Timelines;

namespace EventSystem
{
    public static class TimelineEvents
    {
        public static readonly PlayTimelineEvent PlayTimelineEvent = new PlayTimelineEvent();
        public static readonly StopTimelineEvent StopTimelineEvent = new StopTimelineEvent();
    }

    public class PlayTimelineEvent : GameEvent
    {
        public TimelineType timelineType;
        
        public PlayTimelineEvent Initializer(TimelineType timelineType)
        {
            this.timelineType = timelineType;
            return this;
        }
    }
    
    public class StopTimelineEvent : GameEvent
    { }
}