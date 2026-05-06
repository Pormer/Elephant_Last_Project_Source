using UnityEngine;

namespace Code.Extensions
{
    public static class ArrayExtension
    {
        public static void Shuffle<T>(this T[] array, int cnt)
        {
            for (int i = 0; i < cnt; i++)
            {
                int idx1 = Random.Range(0, array.Length);
                int idx2 = Random.Range(0, array.Length);
                (array[idx1], array[idx2]) = (array[idx2], array[idx1]);
            }
        }
    }
}