using Code.Modules;
using UnityEngine;

namespace Code.Statuses
{
    public class TestStatusHandler : ModuleOwner, IStatusHandler
    {
        [SerializeField] private StatusType testStatusType;
        
        private StatusModule _statusModule;

        protected override void Awake()
        {
            base.Awake();

            _statusModule = GetModule<StatusModule>();
        }

        public void ApplyStatus(StatusType status, float duration)
        {
            _statusModule.ApplyStatus(status, duration);
            _statusModule.OnStatusEnd += HandleStatusEnd;
            
            print("----Current statuses----");
            
            foreach (StatusType statusData in _statusModule.CurrentStatuses)
                print(statusData);
            
            print("------------------------");
        }

        public void RemoveStatus(StatusType type)
        {
            _statusModule.RemoveStatus(type);
        }

        private void HandleStatusEnd(StatusType status)
        {
            _statusModule.OnStatusEnd -= HandleStatusEnd;
            print($"{status} End");
        }

        [ContextMenu("Test Apply")]
        public void TestApply()
        {
            ApplyStatus(testStatusType, 1f);
        }
    }
}