using Code.Combat;
using Code.Combat.Attacks;
using Code.Detectors.Datas;
using UnityEngine;

namespace Code.Summons.Explosions
{
    [CreateAssetMenu(fileName = "Explosion data", menuName = "SO/Explosion Data", order = 0)]
    public class ExplosionDataSO : ScriptableObject
    {
        public int damage;
        public DetectorDataSO damageCasterData;
        public AttackDataSO attackData;
    }
}