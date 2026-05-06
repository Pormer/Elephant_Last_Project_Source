using System;
using System.Collections.Generic;
using Code.ManaSystem;
using Code.Modules;
using Code.Skills.Core;
using Code.Souls.Core;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Players.Modules
{
    public class SkillSlot
    {
        public SkillDataSO skillData;
        public bool isLock;
        
        public SkillSlot(SkillDataSO skillData, bool isLock)
        {
            this.skillData = skillData;
            this.isLock = isLock;
        }
    }
    
    public class PlayerSkillManagement : MonoBehaviour, IModule, IAfterInitModule
    {
        public UnityEvent OnUseSkillEvent;
        public UnityEvent OnEndSkillEvent;
        public event Action<SkillDataSO> OnEquipSkillChangeEvent;
        
        [SerializeField] private GameEventChannelSO skillChannel;
        
        private Player _player;
        [Inject] private SkillManager _skillManager;
        private ManaModule _manaModule;

        public Skill CurrentSkill => GetSkill(CurrentSkillKey);
        public SkillKeyType CurrentSkillKey { get; private set; }

        private Dictionary<SkillKeyType, SkillSlot> _equipSkillDict;
        private Dictionary<SkillDataSO, Skill> _skillDict;
        
        public void Initialize(ModuleOwner owner)
        {
            _equipSkillDict = new Dictionary<SkillKeyType, SkillSlot>();
            _skillDict = new Dictionary<SkillDataSO, Skill>();
            
            _player = owner as Player;
            _manaModule = owner.GetModule<ManaModule>();
            
            Debug.Assert(_player != null, $"Owner is not player: {owner.name}");
            Debug.Assert(_manaModule != null, $"Mana module is not found: {owner.name}");
            Debug.Assert(_skillManager != null, $"skill manager is null: {owner.name}");
        }

        public void AfterInitialize()
        {
            for (int i = 0; i < (int)SkillKeyType.SKILL_END; i++)
            {
                _equipSkillDict.Add((SkillKeyType)i, new SkillSlot(null, false));
            }
            
            skillChannel.AddListener<PlayerSkillEquipEvent>(HandleSkillEquip);
        }

        private void OnDestroy()
        {
            skillChannel.RemoveListener<PlayerSkillEquipEvent>(HandleSkillEquip);
        }
        
        private void HandleSkillEquip(PlayerSkillEquipEvent evt)
        {
            ChangeEquipSkill(evt.targetType, evt.targetSkillData);
        }

        public void SetCurrentSkill(SkillKeyType key) => CurrentSkillKey = key;

        public bool CanUseSkill(Skill skill)
        {
            return !skill.IsUsing 
                   && !skill.IsCoolDown 
                   && skill.SkillData.needManaValue <= _manaModule.GetSoulManaValue(skill.SkillData.soulType);
        }

        public bool TryUseSkill(Skill skill)
        {
            if (!CanUseSkill(skill)) return false;
            
            bool isSkillUsed = skill.UseSkill();

            if (isSkillUsed)
            {
                OnUseSkillEvent?.Invoke();
                var skillData = skill.SkillData;
                _player.PlayerData.UsedMana(skillData.soulType, skillData.needManaValue);
            }
            
            return isSkillUsed;
        }
        
        public Skill GetSkill(SkillKeyType keyType)
        {
            if (!_equipSkillDict.TryGetValue(keyType, out var skillSlot)) return null;
            
            SkillDataSO skillData = skillSlot.skillData;
            return skillData ? TryAddToSkill(skillData) : null;
        }

        private void ChangeEquipSkill(SkillKeyType keyType, SkillDataSO changeSkill)
        {
            if (!_equipSkillDict.TryAdd(keyType, new SkillSlot(changeSkill, false)))
            {
                _equipSkillDict[keyType].skillData = changeSkill;
            }
            
            OnEquipSkillChangeEvent?.Invoke(changeSkill);
        }

        private Skill TryAddToSkill(SkillDataSO skillData)
        {
            Skill skill = GetSkillInstance(skillData);
            
            if(_skillDict.TryGetValue(skillData, out Skill skillIns) && !skillIns)
                _skillDict[skillData] = skill;
            
            _skillDict.TryAdd(skillData, skill);
            return skill;
        }

        private Skill GetSkillInstance(SkillDataSO skillData)
        {
            if(_skillDict.TryGetValue(skillData, out Skill skillIns) && skillIns)
                return skillIns;
            
            skillIns = _skillManager.GetSkill(skillData, _player, transform);
            return skillIns;
        }

        #region Skill Lock section

        public void LockSkillKey(SkillKeyType key, bool isLock)
        {
            SkillSlot slot = _equipSkillDict[key];
            slot.isLock = isLock;
            skillChannel.RaiseEvent(SkillEvents.SkillLockEvent.Initializer(slot.skillData, key, isLock));
        }

        public void LockSkillType(SoulType soulType, bool isLock)
        {
            foreach (KeyValuePair<SkillKeyType, SkillSlot> kvp in _equipSkillDict)
            {
                SkillSlot slot = kvp.Value;
                
                SkillDataSO targetData = slot.skillData;
                if(targetData == null || targetData.soulType != soulType) continue;
                
                slot.isLock = isLock;
                skillChannel.RaiseEvent(SkillEvents.SkillLockEvent.Initializer(targetData, kvp.Key, isLock));
            }
        }
        
        public bool IsLock(SkillKeyType key) => _equipSkillDict.GetValueOrDefault(key) != null && _equipSkillDict[key].isLock;
        public bool IsCurrentSkillLock() => IsLock(CurrentSkillKey);

        #endregion
        
        
    }
}