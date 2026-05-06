namespace Code.Statuses
{
    public interface IStatusHandler
    {
        public void ApplyStatus(StatusType type, float duration);
        public void RemoveStatus(StatusType type);
    }
}