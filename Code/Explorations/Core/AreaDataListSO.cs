using System.Collections.Generic;
using UnityEngine;

namespace Code.Explorations.Core
{
    [CreateAssetMenu(fileName = "Area data list", menuName = "SO/Area/List", order = 0)]
    public class AreaDataListSO : ScriptableObject
    {
        public List<AreaDataSO> dataList;
    }
}