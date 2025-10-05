using UnityEngine;

namespace Utils
{
    public static class Array2DExtensions
    {
        public static T Get<T>(this T[,] array, Vector2Int pos)
        {
            return array[pos.x, pos.y];
        }

        public static void Set<T>(this T[,] array, Vector2Int pos, T value)
        {
            array[pos.x, pos.y] = value;
        }
    }
}