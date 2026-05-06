using UnityEngine;

namespace Code.Explorations.Core
{
    [CreateAssetMenu(fileName = "Area data", menuName = "SO/Area/Data", order = 0)]
    public class AreaDataSO : ScriptableObject
    {
        public string areaName;
    }
}