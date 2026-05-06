using Code.Explorations.Core;

namespace EventSystem
{
    public static class StageEvents
    {
        public static AreaUnlockRequestEvent AreaUnlockRequestEvent = new AreaUnlockRequestEvent();
        public static AreaUnlockEvent AreaUnlockEvent = new AreaUnlockEvent();
    }

    public class AreaUnlockRequestEvent : GameEvent
    {
        public AreaDataSO targetArea;

        public AreaUnlockRequestEvent Initializer(AreaDataSO targetArea)
        {
            this.targetArea = targetArea;
            return this;
        }
    }
    
    public class AreaUnlockEvent : GameEvent
    {
        public AreaDataSO targetArea;

        public AreaUnlockEvent Initializer(AreaDataSO targetArea)
        {
            this.targetArea = targetArea;
            return this;
        }
    }
}