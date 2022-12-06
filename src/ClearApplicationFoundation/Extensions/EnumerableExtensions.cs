using System;
using System.Collections.Generic;

namespace ClearApplicationFoundation.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;
            foreach (var item in enumerable)
            {
                action(item, i++);
            }
        }
    }
}
