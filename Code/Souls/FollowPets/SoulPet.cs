using Code.Contexts.Summons;
using Code.Entities.Modules;
using Code.Summons.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Souls.FollowPets
{
    public class SoulPet : Summon<SoulPetSummonContext>
    {
        public UnityEvent OnPetChangedEvent;
        
        private FollowTargetMovement _followMovement;
        private EntitySpriteLibrary _spriteLibrary;
        
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            
            _spriteLibrary = GetModule<EntitySpriteLibrary>();
            _followMovement = GetModule<FollowTargetMovement>();
            
            Debug.Assert(_spriteLibrary != null, "entity sprite library is null");
            Debug.Assert(_followMovement != null, "follow target movement is null");
        }

        public override void SetUp(SoulPetSummonContext context)
        {
            Release();
            
            base.SetUp(context);
            transform.position = context.Owner.transform.TransformPoint(context.SitOffset);
            
            _spriteLibrary.SetLibrary(context.SpriteLibraryData);
            _followMovement.SetTarget(context.Owner.transform, context.SitOffset);
            
            OnPetChangedEvent?.Invoke();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}