using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensions
{
    public static T Random<T>(this IEnumerable<T> _this)
    {
        return _this.ElementAt(UnityEngine.Random.Range(0, _this.Count()));
    }
}
