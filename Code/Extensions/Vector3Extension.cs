
using UnityEngine;

namespace Code.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 RemoveY(this Vector3 v) => new Vector3(v.x, 0, v.z);

        public static Vector3 MultiplyElements(this Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }
}