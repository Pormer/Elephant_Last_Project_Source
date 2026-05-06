using UnityEngine;

namespace Code.Extensions
{
    public static class GameObjectExtension
    {
        public static bool IsSameLayer(this GameObject gameObject, LayerMask targetLayer)
        {
            return ((1 << gameObject.layer) & targetLayer) > 0;
        }
    }
}