using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Code.Souls.Core
{
    [CreateAssetMenu(fileName = "Soul aura", menuName = "SO/Soul/Aura", order = 0)]
    public class SoulAuraDataSO : ScriptableObject
    {
        public Vector3 auraOffset;
        public Vector3 petOffset;
        public RuntimeAnimatorController auraAnimController;
        public SpriteLibraryAsset petSpriteLibraryData;
    }
}