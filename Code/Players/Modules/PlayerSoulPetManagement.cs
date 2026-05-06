using Code.Contexts.Summons;
using Code.Modules;
using Code.Souls.Core;
using Code.Souls.FollowPets;
using EventSystem;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Code.Players.Modules
{
    public class PlayerSoulPetManagement : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private GameEventChannelSO skillChannel;
        [SerializeField] private SoulPet soulPetPrefab;
        
        private ModuleOwner _owner;
        private Transform _ownerTrm;
        private SoulPet _mainPet;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _ownerTrm = owner.transform;
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
            SetSoulPet(evt.targetSoul.auraData);
        }

        private void SetSoulPet(SoulAuraDataSO auraData)
        {
            if (_mainPet == null) _mainPet = SpawnPet();
            var library = auraData.petSpriteLibraryData;
            
            _mainPet.SetUp(CreateSummonContext(_ownerTrm, library, auraData.petOffset));
        }

        private SoulPet SpawnPet()
        {
            var item = Instantiate(soulPetPrefab);
            
            return item;
        }

        private SoulPetSummonContext CreateSummonContext(Transform target, SpriteLibraryAsset libraryData, Vector3 offset)
        {
            var context = new SoulPetSummonContext
            (
                _owner,
                target.position,
                target.eulerAngles,
                offset,
                libraryData
            );
            
            return context;
        }
    }
}