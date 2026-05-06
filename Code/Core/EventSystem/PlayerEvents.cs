namespace EventSystem
{
    public static class PlayerEvents
    {
        public static readonly PlayerAddManaEvent PlayerAddManaEvent = new PlayerAddManaEvent();
    }
    
    public class PlayerAddManaEvent : GameEvent
    {
        public int manaAmount;
        
        public PlayerAddManaEvent Initializer(int amount)
        {
            manaAmount = amount;
            return this;
        }
    }
}