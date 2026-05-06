using System.Collections.Generic;
using Code.Combat;
using Code.Modules;
using UnityEngine;

namespace Code.Interactable.Destructibles
{
    public class DestructibleRenderer : MonoBehaviour, IModule
    {
        [SerializeField] private GameObject destructible1;
        [SerializeField] private GameObject destructible2;
        [SerializeField] private GameObject destructible3;
        
        private HealthModule _healthModule;
        private Destructible _destructible;
        
        public void Initialize(ModuleOwner owner)
        {
            _destructible = owner as Destructible;
            
            Debug.Assert(_destructible != null, $"Owner is not destructible: {_destructible}");
            
            _healthModule = _destructible.GetModule<HealthModule>();
        }

        public void UpdateDamageModel(int count)
        {
            if (count == 1)
            {
                destructible2.SetActive(true);
                destructible3.SetActive(false);
            }
            else if (count == 2)
            {
                
            }
            else if (count == 3)
            {
                
            }
        }
    }
}