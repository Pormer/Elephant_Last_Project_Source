using System.Collections.Generic;
using Code.Souls.Core;
using Code.UI.InputBind;
using UnityEngine;

namespace Code.UI.InGame
{
    public class SkillList : MonoBehaviour
    {
        [SerializeField] private GameObject skillPrefab;
        [SerializeField] private Transform listRoot;
        [SerializeField] private List<SkillItem> skillList;
        [SerializeField] private UIInputBindSO inputBind;
        
        public List<SkillItem> SkillItems => skillList;

        public SoulDataSO AbilityData { get; private set; }

        public void SetSoul(SoulDataSO data)
        {
            AbilityData = data;
            SyncSkillItemCount(data.skills.Length);
            BindSkills(data);
        }

        private void SyncSkillItemCount(int targetCount)
        {
            for (var i = skillList.Count - 1; i >= targetCount; i--)
            {
                var item = skillList[i];
                skillList.RemoveAt(i);
                Destroy(item.gameObject);
            }

            for (var i = skillList.Count; i < targetCount; i++)
            {
                var item = Instantiate(skillPrefab, listRoot).GetComponent<SkillItem>();
                skillList.Add(item);
            }
        }

        private void BindSkills(SoulDataSO data)
        {
            for (var i = 0; i < data.skills.Length; i++)
            {
                var skillData = data.skills[i];
                var skillItem = skillList[i];

                skillItem.SetSkill(skillData.targetSkill);
            }
        }
    }
}
