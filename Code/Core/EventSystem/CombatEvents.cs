using Code.Stage;

namespace EventSystem
{
    public static class CombatEvents
    {
        public static readonly CombatStartEvent CombatStartEvent = new CombatStartEvent();
        public static readonly CombatEndEvent CombatEndEvent = new CombatEndEvent();
    }

    public class CombatStartEvent : GameEvent
    {
    }
    
    public class CombatEndEvent : GameEvent
    {
    }
}