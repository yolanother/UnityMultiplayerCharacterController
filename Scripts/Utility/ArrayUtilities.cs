using System;

namespace DoubTech.MCC.Utilities
{
    public static class ArrayUtilities
    {
        public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
    }
}