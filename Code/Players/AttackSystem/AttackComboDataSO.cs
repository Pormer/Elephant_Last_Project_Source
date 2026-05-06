using UnityEngine;

namespace Code.Players.AttackSystem
{
    [CreateAssetMenu(fileName = "AttackComboData", menuName = "SO/Combat/AttackComboData", order = 0)]
    public class AttackComboDataSO : ScriptableObject
    {
        public AttackComboType comboType;
        
        public AttackActionDataSO[] actions;
        public AttackActionDataSO this[int index] => actions[index];
        
        public int MaxComboCount => actions.Length;
    }
}