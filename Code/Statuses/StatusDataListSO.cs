using System.Collections.Generic;
using UnityEngine;

namespace Code.Statuses
{
    [CreateAssetMenu(fileName = "Status data list", menuName = "SO/Status/List", order = 0)]
    public class StatusDataListSO : ScriptableObject
    {
        public List<StatusDataSO> statusList;
    }
}