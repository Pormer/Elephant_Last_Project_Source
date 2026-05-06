using UnityEngine;

namespace Code.Combat
{
    [CreateAssetMenu(fileName = "CombatInfo", menuName = "SO/Combat/CombatInfo", order = 0)]
    public class CombatInfoSO : ScriptableObject
    {
        public bool isCombat;
    }
}