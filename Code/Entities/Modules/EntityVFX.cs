using System.Collections.Generic;
using System.Linq;
using Code.Effects;
using Code.Modules;
using UnityEngine;

namespace Code.Entities.Modules
{
    public class EntityVFX : MonoBehaviour, IModule
    {
        private Dictionary<string, IPlayableVFX> _playableDictionary;
        
        public void Initialize(ModuleOwner owner)
        {
            _playableDictionary = new Dictionary<string, IPlayableVFX>();
            GetComponentsInChildren<IPlayableVFX>().ToList()
                .ForEach(playable => _playableDictionary.Add(playable.VFXName, playable));
        }

        public void PlayVFX(string vfxName, Vector3 position, Quaternion rotation)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            
            vfx.PlayVFX(position, rotation);
        }

        public void StopVFX(string vfxName)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            
            vfx.StopVFX();
        }
    }
}