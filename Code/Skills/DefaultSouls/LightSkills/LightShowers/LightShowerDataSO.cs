using Code.Combat;
using Code.Combat.Attacks;
using Code.Detectors.Datas;
using Code.Skills.Core;
using Code.Utility.Properties;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Skills.DefaultSouls.LightSkills.LightShowers
{
    [CreateAssetMenu(fileName = "Light shower data", menuName = "SO/Skill/LightShower", order = 0)]
    public class LightShowerDataSO : SkillDataSO
    {
        [Header("References")]
        public GameObject lightShowerPrefab;
        public PoolItemSO finalMeteorItem;
        public PoolItemSO meteorItem;
        public DetectorDataSO meteorCasterData;
        public AttackDataSO meteorAttackData;
        public PropertyDataSO dissolveProperty;
        
        [Header("Light shower setting")]
        public float showerRange;
        public float detectRange;
        public float ringDissolveStartDelay;
        public float ringDissolveDuration;
        
        [Header("Light meteor spawn setting")]
        public float meteorSpawnMinRadius;
        public float meteorSpawnMaxRadius;
        public int meteorCnt;
        public float meteorTerm;
        public float finalMeteorTerm;
    }
}