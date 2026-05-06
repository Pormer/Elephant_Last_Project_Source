using System;
using System.Collections.Generic;

namespace Code.Statuses
{
    public interface IManageStatus
    {
        public event Action<StatusType> OnStatusApply;
        public event Action<StatusType> OnStatusRefresh;
        public event Action<StatusType> OnStatusEnd;
        public HashSet<StatusType> CurrentStatuses { get; }
        public bool HasStatuses { get; }
        public bool ApplyStatus(StatusType status, float duration);
        public void RemoveStatus(StatusType status);
        public void ClearStatus();
    }
}