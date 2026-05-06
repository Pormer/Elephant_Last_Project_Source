using System;
using System.Globalization;
using Code.Skills;
using Code.Skills.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame.MainHud
{
    public class SkillCoolTimeIcon : MonoBehaviour
    {
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image coolTimeIcon;
        [SerializeField] private TextMeshProUGUI coolTimeText;

        public Skill TargetData { get; private set; }

        private void LateUpdate()
        {
            if (TargetData == null) return;
            SetCoolTime(TargetData.Cooldown, TargetData.CooldownTimer);
        }

        public void SetSkill(Skill skill)
        {
            TargetData = skill;
            skillIcon.sprite = skill.SkillData.Icon;
        }

        private void SetCoolTime(float coolTime, float timer)
        {
            if (timer <= 0f && coolTimeIcon.fillAmount <= 0f) return;

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                coolTimeIcon.fillAmount = 0f;
                coolTimeText.text = "";
            }
            else
            {
                coolTimeIcon.fillAmount = timer / coolTime;
                coolTimeText.text = timer.ToString("F1", CultureInfo.InvariantCulture);
            }
        }
    }
}