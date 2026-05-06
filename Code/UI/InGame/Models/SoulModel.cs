using System.Collections.Generic;
using System.Linq;
using Code.Collectibles.Inventories;
using Code.Players;
using Code.Players.Modules;
using Code.Skills.Core;
using Code.Souls.Core;
using Cysharp.Threading.Tasks;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

// using Code.Skills;

namespace Code.UI.InGame.Models
{
    public enum SoulUIProperty
    {
        SoulList,
        FocusedSoul,
        FocusedSkill,
        ChosenSoulItem,
        CurrentSoulSlot,
        CurrentSkills
    }

    public class SoulModel : ModelBase
    {
        public UnityEvent OnEquipSoul;
        
        [SerializeField] private GameEventChannelSO skillChannel;
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private bool startWithAllSouls;
#endif
        [Inject] private SoulInventory _soulInventory;
        [Inject] private Player _player;

        #region Properties

        public List<SoulDataSO> SoulDatas
        {
            get => GetProperty<List<SoulDataSO>>(SoulUIProperty.SoulList);
            set => SetProperty(SoulUIProperty.SoulList, value);
        }

        public SoulDataSO FocusedSoul
        {
            get => GetProperty<SoulDataSO>(SoulUIProperty.FocusedSoul);
            set => SetProperty(SoulUIProperty.FocusedSoul, value, true);
        }

        public SkillDataSO FocusedSkill
        {
            get => GetProperty<SkillDataSO>(SoulUIProperty.FocusedSkill);
            set => SetProperty(SoulUIProperty.FocusedSkill, value);
        }

        public SoulDataSO ChosenSoul
        {
            get => GetProperty<SoulDataSO>(SoulUIProperty.ChosenSoulItem);
            set => SetProperty(SoulUIProperty.ChosenSoulItem, value);
        }

        public Dictionary<SoulType, SoulDataSO> CurrentSoulSlot
        {
            get => GetProperty<Dictionary<SoulType, SoulDataSO>>(SoulUIProperty.CurrentSoulSlot);
            set => SetProperty(SoulUIProperty.CurrentSoulSlot, value, true);
        }

        public List<Skill> CurrentSkills
        {
            get=>GetProperty<List<Skill>>(SoulUIProperty.CurrentSkills);
            set => SetProperty(SoulUIProperty.CurrentSkills, value);
        }

        #endregion
        
        private PlayerSkillManagement _playerSkillManagement;


        protected override async void Initialize()
        {
            base.Initialize();
            FocusedSoul = null;
            ChosenSoul = null;
            FocusedSkill = null;
            SoulDatas = new List<SoulDataSO>();
            CurrentSoulSlot = new Dictionary<SoulType, SoulDataSO>();
            CurrentSkills = new List<Skill>();
            
            if(_player != null)
            {
                await UniTask.WaitForEndOfFrame();
                _playerSkillManagement = _player.GetModule<PlayerSkillManagement>();
                if(_playerSkillManagement == null) Debug.LogError("[SkillModel] Can't find PlayerSkillManagement module");
            }
            
            _soulInventory.OnGetSoulEvent += HandleGetSoul;
            skillChannel.AddListener<PlayerSoulChangeEvent>(HandleSoulSwap);

#if UNITY_EDITOR
            if (startWithAllSouls)
            {
                await UniTask.WaitUntil(() => _soulInventory.Items != null);
                SoulDatas = _soulInventory.Items.ToList();
            }
#endif
        }

        private void OnDestroy()
        {
            _soulInventory.OnGetSoulEvent -= HandleGetSoul;
            skillChannel.RemoveListener<PlayerSoulChangeEvent>(HandleSoulSwap);
        }

        private void HandleSoulSwap(PlayerSoulChangeEvent obj)
        {
            if(_playerSkillManagement == null) return;
            var skills = new Skill[3];
            foreach (var target in obj.targetSoul.skills)
            {
                if (target.keyType == SkillKeyType.Dash)
                {
                    Debug.Log($"[SkillModel] 아직 대쉬 UI 안 만듦 :#");
                    continue;
                }
                var tarSkill = _playerSkillManagement.GetSkill(target.keyType);
                if (tarSkill == null)
                    Debug.LogError($"[SkillModel] Tried to find {target.keyType} skill in PlayerSkillManagement module but failed");
                skills[(int)target.keyType] = tarSkill;
            }
            
            CurrentSkills = skills.ToList();
        }

        private void HandleGetSoul(SoulDataSO obj)
        {
            if (SoulDatas == null)
            {
                SoulDatas = new List<SoulDataSO>();
            }

            MutateProperty<List<SoulDataSO>>(SoulUIProperty.SoulList, souls => souls.Add(obj));
        }

        public void Equip(SoulDataSO soul)
        {
            if (soul == null)
            {
                SetEmpty();
                return;
            }

            ChosenSoul = null;

            if (CurrentSoulSlot == null)
            {
                CurrentSoulSlot = new Dictionary<SoulType, SoulDataSO>();
            }

            MutateProperty<Dictionary<SoulType, SoulDataSO>>(SoulUIProperty.CurrentSoulSlot,
                slots => slots[soul.soulType] = soul);
            skillChannel.RaiseEvent(SkillEvents.PlayerSoulEquipEvent.Initializer(soul.soulType, soul));
            OnEquipSoul?.Invoke();
        }

        private void SetEmpty()
        {
            //TODO: 빈칸 0_0
        }
    }
}
