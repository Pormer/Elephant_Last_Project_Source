using System.Collections.Generic;
using UnityEngine;

namespace Code.Souls.Core
{
    [CreateAssetMenu(fileName = "SoulDataListData", menuName = "SO/Soul/DataList", order = 0)]
    public class SoulDataListSO : ScriptableObject
    {
        public List<SoulDataSO> values;
    }
}