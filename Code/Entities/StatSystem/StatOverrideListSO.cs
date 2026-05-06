using UnityEngine;

namespace Code.Entities.StatSystem
{
    [CreateAssetMenu(fileName = "StatOverrideList", menuName = "SO/Stat/StatOverrideList")]
    public class StatOverrideListSO : ScriptableObject
    {
        public StatOverride[] statOverrides;
    }
}