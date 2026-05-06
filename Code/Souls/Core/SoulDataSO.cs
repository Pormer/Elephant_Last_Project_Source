using Code.Collectibles.Core;
using Code.Skills.Core;
using UnityEngine;

namespace Code.Souls.Core
{
    [CreateAssetMenu(fileName = "Soul data", menuName = "SO/Soul/Data", order = 0)]
    public class SoulDataSO : CollectibleDataSO
    {
        [Header("Visual")]
        public Sprite soulItemIcon;
        
        [Header("Information")]
        public SkillKeyMatchData[] skills = new SkillKeyMatchData[3];
        public SoulType soulType;
        
        //웬만해선 밝은 계열 색으로...
        public Color soulColor;
        
        [Header("Aura")] 
        public SoulAuraDataSO auraData;
    }
}