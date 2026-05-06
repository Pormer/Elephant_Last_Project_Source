using System.Collections.Generic;
using Code.Modules;
using Code.Skills.Core;
using Code.Souls.Core;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Players.Modules
{
    public class PlayerSoulManagement : MonoBehaviour, IModule, IAfterInitModule
    {
        public UnityEvent OnActiveSoulChangedEvent;
        [SerializeField] private GameEventChannelSO skillChannel;

        public SoulType CurrentSlotType { get; private set; }
        public SoulDataSO CurrentActiveSoul => _slotDict.GetValueOrDefault(CurrentSlotType);
        
        private Dictionary<SoulType, SoulDataSO> _slotDict;
        
        public void Initialize(ModuleOwner owner)
        {
            _slotDict = new Dictionary<SoulType, SoulDataSO>();
        }
        
        public void AfterInitialize()
        {
            for (int i = 0; i < (int)SoulType.END; i++)
            {
                _slotDict.Add((SoulType)i, null);
            }
            
            skillChannel.AddListener<PlayerSoulEquipEvent>(HandleSoulEquip);
        }

        private void OnDestroy()
        {
            skillChannel.RemoveListener<PlayerSoulEquipEvent>(HandleSoulEquip);
        }

        private void HandleSoulEquip(PlayerSoulEquipEvent evt)
        {
            EquipSoul(evt.targetType, evt.targetSoul);
        }

        private void EquipSoul(SoulType type, SoulDataSO soul)
        {
            _slotDict[type] = soul;

            if(CurrentSlotType == SoulType.None)
                CurrentSlotType = type;
                
            if (CurrentSlotType == type)
            {
                if (soul == null) return;
                ActiveSkill(soul);
            }
        }

        public void ChangeSlot()
        {
            var tempSlot = CurrentSlotType == SoulType.God ? SoulType.Devil : SoulType.God;

            if (!CanChangeSlot())
                return;

            CurrentSlotType = tempSlot;
            ActiveSkill(CurrentActiveSoul);
        }

        public bool CanChangeSlot()
        {
            var tempSlot = CurrentSlotType == SoulType.God ? SoulType.Devil : SoulType.God;

            return _slotDict.TryGetValue(tempSlot, out SoulDataSO soul)
                   && soul != null
                   && CurrentSlotType != tempSlot;
        }

        private void ActiveSkill(SoulDataSO soul)
        {
            if (soul == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Player soul management: Call equip active skill method => Soul is null");
#endif
                return;
            }
            
            foreach (var matchData in soul.skills)
            {
                EquipSkillToKey(matchData.keyType, matchData.targetSkill);
            }
            
            var changeEvt = 
                SkillEvents.PlayerSoulChangeEvent.Initializer(soul);
            skillChannel.RaiseEvent(changeEvt);
            
            OnActiveSoulChangedEvent?.Invoke();
        }

        private void EquipSkillToKey(SkillKeyType keyType, SkillDataSO skillData)
        {
            var evt = 
                SkillEvents.PlayerSkillEquipEvent.Initializer(keyType, skillData);
            skillChannel.RaiseEvent(evt);
        }
    }
}