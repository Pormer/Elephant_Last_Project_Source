using System;
using UnityEngine;

namespace Code.Entities.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatPair statPair;

        public StatSO Stat => statPair.stat;
        public float Value => statPair.value;
        public StatOverride(StatSO stat) => statPair.stat = stat;

        public StatSO CreateStat()
        {
            StatSO newStat = statPair.stat.Clone() as StatSO;
            Debug.Assert(newStat != null, $"{nameof(newStat)} stat clone failed");

            newStat.BaseValue = statPair.value;

            return newStat;
        }
    }
}