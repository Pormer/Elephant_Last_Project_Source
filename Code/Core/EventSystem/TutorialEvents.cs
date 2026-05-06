using Code.Tutorials;

namespace EventSystem
{
    public static class TutorialEvents
    {
        public static readonly StartTutorialEvent StartTutorialEvent = new StartTutorialEvent();
        public static readonly EndTutorialEvent EndTutorialEvent = new EndTutorialEvent();
    }

    public class StartTutorialEvent : GameEvent
    {
        public TutorialType tutorialType;
        public float endDelay;
        
        public StartTutorialEvent Initializer(TutorialType tutorialType, float endDelay = -1)
        {
            this.tutorialType = tutorialType;
            this.endDelay = endDelay;
            return this;
        }
    }
    
    public class EndTutorialEvent : GameEvent
    {
        public TutorialType tutorialType;
        
        public EndTutorialEvent Initializer(TutorialType tutorialType)
        {
            this.tutorialType = tutorialType;
            return this;
        }
    }
}