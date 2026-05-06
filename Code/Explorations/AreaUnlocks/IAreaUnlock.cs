using Code.Explorations.Core;

namespace Code.Explorations.AreaUnlocks
{
    public interface IAreaUnlock
    {
        public AreaDataSO TargetArea { get; }
        public void RequestUnlock();
    }
}