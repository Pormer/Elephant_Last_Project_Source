using Code.Collectibles.Core;
using Code.Entities.StatSystem;
using Code.Souls.Core;
using UnityEngine;

namespace Code.Skills.Core
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SO/Skill/SkillDataSO", order = 0)]
    public class SkillDataSO : CollectibleDataSO
    {
        [Tooltip("스킬 사용시 상태를 변경 여부")] public bool useChangeState = true;
        public string skillStateName = "SKILL";
        public StatSO majorDamageStat; // 신 스킬 : 신 데미지 스탯, 악마 스킬 : 악마 데미지 스탯
        public SoulType soulType;
        
        public float cooldown;
        [Range(0, 100)] public int needManaValue;
    }
}