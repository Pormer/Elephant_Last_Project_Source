using Code.Modules;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Code.Entities.Modules
{
    [RequireComponent(typeof(SpriteLibrary))]
    public class EntitySpriteLibrary : MonoBehaviour, IModule
    {
        private SpriteLibrary _spriteLibrary;
        
        public void Initialize(ModuleOwner owner)
        {
            _spriteLibrary = GetComponent<SpriteLibrary>();
            
        }

        public SpriteLibraryAsset CurrentLibraryData => _spriteLibrary.spriteLibraryAsset;

        public void SetLibrary(SpriteLibraryAsset asset)
        {
            _spriteLibrary.spriteLibraryAsset = asset;
        }
    }
}