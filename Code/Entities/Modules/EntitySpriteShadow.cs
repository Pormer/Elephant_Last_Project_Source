using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Entities.Modules
{
    public class EntitySpriteShadow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetSpriter;
        
        private void Awake()
        {
            SetApplyShadow();
        }

        private void OnValidate()
        {
            SetApplyShadow();
        }

        private void SetApplyShadow()
        {
            if (targetSpriter == null) return;
            
            targetSpriter.shadowCastingMode = ShadowCastingMode.On;
            targetSpriter.receiveShadows = true;
        }
    }
}