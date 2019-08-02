using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TsinghuaNet.Models;

namespace TsinghuaNet
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

        public static IEnumerable<T> Supplement<T>(this IEnumerable<T> source, int startIndex, Func<T, int> intSelector, Func<int, T> valueSelector)
            => SupplementIterator(source ?? throw new ArgumentNullException(nameof(source)), startIndex, int.MinValue, intSelector, valueSelector);

        public static IEnumerable<T> Supplement<T>(this IEnumerable<T> source, int startIndex, int endIndex, Func<T, int> intSelector, Func<int, T> valueSelector)
            => SupplementIterator(source ?? throw new ArgumentNullException(nameof(source)), startIndex, endIndex, intSelector, valueSelector);

        private static IEnumerable<T> SupplementIterator<T>(IEnumerable<T> source, int startIndex, int endIndex, Func<T, int> intSelector, Func<int, T> valueSelector)
        {
            int lastInt = startIndex - 1;
            foreach (T value in source)
            {
                int currentInt = intSelector(value);
                while (currentInt - lastInt > 1)
                    yield return valueSelector(++lastInt);
                lastInt = currentInt;
                yield return value;
            }
            while (lastInt < endIndex)
                yield return valueSelector(++lastInt);
        }
    }
}
