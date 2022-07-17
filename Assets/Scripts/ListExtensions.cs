
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i >= 1; --i)
        {
            var k = Random.Range(0, i + 1);
            var tmp = list[k];
            list[k] = list[i];
            list[i] = tmp;
        }
    }
}
