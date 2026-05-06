using System;
using System.Collections.Generic;
using Code.Combat;
using Code.Entities;
using Code.ManaSystem;
using Code.Modules;
using Code.Souls.Core;
using EventSystem;
using UnityEngine;

namespace Code.Players
{
    public class PlayerData : MonoBehaviour, IModule
    {
        public delegate void PlayerDataValueChanged<in T>(T current, T max); //current, prev or current, max


        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        public event PlayerDataValueChanged<int> OnPlayerHealthChanged;
        public event Action OnPlayerManaChanged;
        
        [field: SerializeField] public int MaxManaValue { get; private set; }
        [field: SerializeField] public int HealthValue { get; private set; }
        [field: SerializeField] public int MaxHealthValue { get; private set; }
        [field: SerializeField] public int ComboCount { get; private set; }

        public int GodManaValue => _soulManaDict.GetValueOrDefault(SoulType.God);
        public int DevilManaValue => _soulManaDict.GetValueOrDefault(SoulType.Devil);
        public int HybridManaValue => _soulManaDict.GetValueOrDefault(SoulType.HYBRID);
        
        private Dictionary<SoulType, PlayerDataValueChanged<int>> _soulManaChangeActionDict;
        private Dictionary<SoulType, int> _soulManaDict;
        
        private Entity _player;
        private HealthModule _healthModule;
        private ManaModule _manaModule;

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Entity;

            _healthModule = owner.GetModule<HealthModule>();
            _manaModule = owner.GetModule<ManaModule>();

            Debug.Assert(_player != null, $"owner is not player: {owner.name}");
            Debug.Assert(_healthModule != null, $"health module is not fount: {owner.name}");
            Debug.Assert(_manaModule != null, $"mana module is not fount: {owner.name}");

            _soulManaDict = new Dictionary<SoulType, int>();
            _soulManaChangeActionDict = new Dictionary<SoulType, PlayerDataValueChanged<int>>();

            for (int i = 0; i < (int)SoulType.END; i++)
            {
                _soulManaDict.Add((SoulType)i, 0);
                _soulManaChangeActionDict.Add((SoulType)i, null);
            }

            _healthModule.OnHealthChangeEvent.AddListener(HandleHealthChanged);
            PlayerChannel.AddListener<PlayerAddManaEvent>(HandleAddMana);
            _manaModule.OnManaValueChanged += HandleManaValueChanged;
        }

        private void OnDestroy()
        {
            _healthModule.OnHealthChangeEvent.RemoveListener(HandleHealthChanged);
            PlayerChannel.RemoveListener<PlayerAddManaEvent>(HandleAddMana);
            _manaModule.OnManaValueChanged -= HandleManaValueChanged;
        }

        private void HandleHealthChanged(int currentValue, int maxValue)
        {
            HealthValue = currentValue;
            MaxHealthValue = maxValue;

            OnPlayerHealthChanged?.Invoke(HealthValue, MaxHealthValue);
        }

        private void HandleManaValueChanged(SoulType type, int currentValue, int maxValue)
        {
            if(_soulManaDict.ContainsKey(type)) 
                _soulManaDict[type] = currentValue;

            if (_soulManaChangeActionDict.TryGetValue(type, out var action))
                action?.Invoke(currentValue, maxValue);

            OnPlayerManaChanged?.Invoke();
            MaxManaValue = maxValue;
        }

        private void HandleAddMana(PlayerAddManaEvent evt)
        {
            for (int i = 0; i < (int)SoulType.END; i++)
            {
                AddMana((SoulType)i, evt.manaAmount);
            }
        }

        public void AddMana(SoulType type, int amount) => _manaModule.AddManaValue(type, amount);
        public void UsedMana(SoulType type, int amount) => _manaModule.UsedManaValue(type, amount);

        public void AddManaChangeListener(SoulType type, PlayerDataValueChanged<int> handler)
        {
            if (_soulManaChangeActionDict.TryGetValue(type, out var action))
            {
                action += handler;
                _soulManaChangeActionDict[type] = action;
            }
        }
        public void RemoveManaChangeListener(SoulType type, PlayerDataValueChanged<int> handler)
        {
            if (_soulManaChangeActionDict.TryGetValue(type, out var action))
            {
                action -= handler;
                _soulManaChangeActionDict[type] = action;
            }
        }
    }
}