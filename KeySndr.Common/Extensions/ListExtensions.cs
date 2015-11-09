using System.Collections.Generic;

namespace KeySndr.Common.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> l, int a, int b)
        {
            var tmp = l[a];
            l[a] = l[b];
            l[b] = tmp;
        }
    }
}
