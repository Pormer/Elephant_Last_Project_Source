namespace Code.Skills.Core
{
    public interface IChargeable
    {
        bool IsCharging { get; }
        void StartCharging();
        void ReleaseCharging();
        void CancelCharging();
    }
}