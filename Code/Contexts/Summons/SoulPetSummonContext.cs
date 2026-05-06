using Code.Modules;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Code.Contexts.Summons
{
    public class SoulPetSummonContext : SummonContext
    {
        public SpriteLibraryAsset SpriteLibraryData { get; private set; }
        public Vector3 SitOffset  { get; private set; }

        public SoulPetSummonContext(ModuleOwner owner, Vector3 position, Vector3 rotation, Vector3 sitOffset, SpriteLibraryAsset spriteLibraryData) : base(owner, position, rotation)
        {
            SpriteLibraryData = spriteLibraryData;
            SitOffset = sitOffset;
        }
    }
}