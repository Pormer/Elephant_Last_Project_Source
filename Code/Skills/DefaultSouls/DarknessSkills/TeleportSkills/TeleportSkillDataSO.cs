using Code.Combat;
using Code.Combat.Attacks;
using Code.Detectors.Datas;
using Code.Skills.Core;
using Code.Utility.Properties;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Skills.DefaultSouls.DarknessSkills.TeleportSkills
{
    [CreateAssetMenu(fileName = "Teleport skill data", menuName = "SO/Skill/TeleportSkill", order = 0)]
    public class TeleportSkillDataSO : SkillDataSO
    {
        [Header("References")] 
        public GameObject riseObjectPrefab;
        public DetectorDataSO riseUpCasterData;
        public DetectorDataSO detectorData;
        public AttackDataSO riseUpAttackData;
        public PropertyDataSO phaseColorData;
        public PropertyDataSO splitData;
        
        [Header("Teleport settings")] 
        public LayerMask obstacleLayer;
        public float rayDistance;
        public float teleportOffset;

        [Header("visual settings")] 
        public PoolItemSO afterImageItem;
        [ColorUsage(true, true)] public Color phaseColor;
        public float phaseDuration;
    }
}