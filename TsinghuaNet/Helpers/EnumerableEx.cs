using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TsinghuaNet.Helpers
{
    public static class EnumerableEx
    {
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool descending)
            => descending ? source.OrderByDescending(selector) : source.OrderBy(selector);

        public static IOrderedAsyncEnumerable<T> OrderBy<T, TKey>(this IAsyncEnumerable<T> source, Func<T, TKey> selector, bool descending)
            => descending ? source.OrderByDescending(selector) : source.OrderBy(selector);

        public static ByteSize Sum(this IEnumerable<ByteSize> source)
            => new ByteSize(source.Sum(s => s.Bytes));

        public async static ValueTask<ByteSize> SumAsync(this IAsyncEnumerable<ByteSize> source)
            => new ByteSize(await source.SumAsync(s => s.Bytes));
    }
}
