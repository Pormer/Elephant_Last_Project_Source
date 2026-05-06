using Code.Core;
using UnityEngine;

namespace Code.Statuses
{
    [CreateAssetMenu(fileName = "Status data", menuName = "SO/Status/Data", order = 0)]
    public class StatusDataSO : DisplayableSO
    {
        public int priority; //stack type이 non stackable일때만 적용됨 (cc기만 우선순위 판별)
        public StatusStackType stackType;
        public StatusType statusType;
    }
}