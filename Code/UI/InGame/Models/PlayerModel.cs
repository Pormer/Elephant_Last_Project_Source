using Code.Combat;
using Code.Players;
using Code.Souls.Core;
using Cysharp.Threading.Tasks;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.UI.InGame.Models
{
    public enum PlayerUIProperty
    {
        PlayerHealth,
        PlayerMaxHealth,
        PlayerMaxMana,
        PlayerGodMana,
        PlayerDevilMana,
    }
    public class PlayerModel : ModelBase
    {
        [Inject] private Player _player;
        private PlayerData _playerData;
        public int PlayerHealth
        {
            get => GetProperty<int>(PlayerUIProperty.PlayerHealth); 
            set => SetProperty(PlayerUIProperty.PlayerHealth, value);
        }

        public int PlayerMaxHealth
        {
            get => GetProperty<int>(PlayerUIProperty.PlayerMaxHealth);
            set => SetProperty(PlayerUIProperty.PlayerMaxHealth, value);
        }

        public int PlayerMaxMana
        {
            get=>GetProperty<int>(PlayerUIProperty.PlayerMaxMana);
            set => SetProperty(PlayerUIProperty.PlayerMaxMana, value);
        }

        public int PlayerGodMana
        {
            get => GetProperty<int>(PlayerUIProperty.PlayerGodMana);
            set => SetProperty(PlayerUIProperty.PlayerGodMana, value);
        }

        public int PlayerDevilMana
        {
            get => GetProperty<int>(PlayerUIProperty.PlayerDevilMana);
            set => SetProperty(PlayerUIProperty.PlayerDevilMana, value);
        }

        protected override async void Initialize()
        {
            base.Initialize();
            if (_player == null) return;

            await UniTask.WaitForEndOfFrame();
            _playerData = _player.PlayerData;
            if (_playerData == null)
            {
                Debug.LogError("[PlayerModel] PlayerData is null.");
                return;
            }

            PlayerHealth = _playerData.HealthValue;
            PlayerMaxHealth = _playerData.MaxHealthValue;
            _playerData.OnPlayerHealthChanged += HandleHealthChange;
            
            PlayerMaxMana = _playerData.MaxManaValue;
            
            PlayerGodMana = _playerData.GodManaValue;
            _playerData.AddManaChangeListener(SoulType.God, HandleGodManaChange);

            PlayerDevilMana = _playerData.DevilManaValue;
            _playerData.AddManaChangeListener(SoulType.Devil, HandleDevilManaChange);
        }

        private void HandleGodManaChange(int current, int nextValue)
        {
            PlayerMaxMana = nextValue;
            PlayerGodMana = current;
        }

        private void HandleDevilManaChange(int current, int nextValue)
        {
            PlayerMaxMana = nextValue;
            PlayerDevilMana = current;
        }

        private void HandleHealthChange(int current, int nextValue)
        {
            PlayerHealth = current;
            PlayerMaxHealth = nextValue;
        }

        private void OnDestroy()
        {
            _playerData.OnPlayerHealthChanged -= HandleHealthChange;
            _playerData.RemoveManaChangeListener(SoulType.God, HandleGodManaChange);
            _playerData.RemoveManaChangeListener(SoulType.Devil, HandleDevilManaChange);
        }
    }
}
