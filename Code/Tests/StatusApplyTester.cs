using Code.Modules;
using Code.Statuses;
using UnityEngine;

namespace Code.Tests
{
    public class StatusApplyTester : MonoBehaviour
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private StatusType wantStatusType;
        [SerializeField] private ModuleOwner target;
        
        [ContextMenu("Apply")]
        private void Apply()
        {
            if(target is IStatusHandler statusHandler)
            {
                statusHandler.ApplyStatus(wantStatusType, duration);
                Debug.Log("상태를 적용했습니다.");
            }
            else
            {
                Debug.Log("상태이상을 적용할 수 없는 객체입니다.");
            }
        }
    }
}