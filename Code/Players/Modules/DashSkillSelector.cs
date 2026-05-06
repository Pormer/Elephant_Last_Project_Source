using System.Collections.Generic;
using Code.Modules;
using Code.Skills.Core;
using Code.Souls.Core;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Players.Modules
{
    public class DashSkillSelector : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private GameEventChannelSO skillChannel;
        [SerializeField] private SkillDataSO defaultDashData;
        [SerializeField] private List<SkillDataSO> dashSkillDataList;
        
        public Skill CurrentActiveDashSkill { get; private set; }
        [Inject] private SkillManager _skillManager;
        private Dictionary<SoulType, Skill> _dashTypeDict;
        
        public void Initialize(ModuleOwner owner)
        {
            _dashTypeDict = new Dictionary<SoulType, Skill>();
            
            foreach (var skillData in dashSkillDataList)
            {
                var skill = _skillManager.GetSkill(skillData, owner, transform);
                _dashTypeDict.TryAdd(skillData.soulType, skill);
            }

            if (!_dashTypeDict.ContainsKey(SoulType.None))
            {
                var skill = _skillManager.GetSkill(defaultDashData, owner, transform);
                _dashTypeDict.Add(SoulType.None, skill);
            }
            
            CurrentActiveDashSkill = GetDashAsType(SoulType.None);
        }
        
        public void AfterInitialize()
        {
            skillChannel.AddListener<PlayerSoulChangeEvent>(HandleSoulChange);
        }

        private void OnDestroy()
        {
            skillChannel.RemoveListener<PlayerSoulChangeEvent>(HandleSoulChange);
        }

        private void HandleSoulChange(PlayerSoulChangeEvent evt)
        {
            CurrentActiveDashSkill = GetDashAsType(evt.targetSoul.soulType);
        }

        public Skill GetDashAsType(SoulType type)
        {
            return _dashTypeDict.GetValueOrDefault(type);
        }
    }
}