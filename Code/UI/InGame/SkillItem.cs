using Code.Skills.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class SkillItem : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public SkillDataSO SkillData { get; private set; }

        public void SetSkill(SkillDataSO skillData)
        {
            SkillData = skillData;
            icon.sprite = skillData == null ? null : skillData.Icon;
        }
    }
}