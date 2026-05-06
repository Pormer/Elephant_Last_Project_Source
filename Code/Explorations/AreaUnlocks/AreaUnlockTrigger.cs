using Code.Explorations.Core;
using Code.Modules;
using EventSystem;
using UnityEngine;

namespace Code.Explorations.AreaUnlocks
{
    public abstract class AreaUnlockTrigger : ModuleOwner, IAreaUnlock
    {
        [field:SerializeField] public AreaDataSO TargetArea { get; protected set; }
        [SerializeField] protected GameEventChannelSO stageChannel;

        public virtual void RequestUnlock()
        {
            stageChannel.RaiseEvent(StageEvents.AreaUnlockRequestEvent.Initializer(TargetArea));
        }
    }
}