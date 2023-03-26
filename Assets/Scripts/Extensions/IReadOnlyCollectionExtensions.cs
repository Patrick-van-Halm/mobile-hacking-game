using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IReadOnlyCollectionExtensions
{
    public static bool Contains<T>(this IReadOnlyCollection<T> haystack, T needle)
    {
        List<T> list = haystack.ToList();
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(needle)) return true;
        }
        return false;
    }
}
