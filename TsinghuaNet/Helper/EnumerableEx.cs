using System;
using System.Collections.Generic;
using System.Linq;

namespace TsinghuaNet.Helper
{
    public static class EnumerableEx
    {
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool descending)
            => descending ? source.OrderByDescending(selector) : source.OrderBy(selector);

        public static ByteSize Sum<T>(this IEnumerable<T> source, Func<T, ByteSize> selector)
            => new ByteSize(source.Sum(v => selector(v).Bytes));
    }
}
