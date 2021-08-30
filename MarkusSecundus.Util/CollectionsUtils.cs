using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Static class with utility functions that should have been in standard library.
    /// </summary>
    public static class CollectionsUtils
    {
        /// <summary>
        /// Push item to the end of the provided list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The list to be pushed to</param>
        /// <param name="toPush">Value to be pushed</param>
        /// <returns><code>self</code> for chaining purposes</returns>
        public static IList<T> Push<T>(this IList<T> self, T toPush)
        {
            self.Add(toPush);
            return self;
        }

        /// <summary>
        /// Remove an item from the end of the provided list and get its value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The list to be popped from</param>
        /// <returns>The removed item</returns>
        public static T Pop<T>(this IList<T> self)
        {
            var ret = self.Peek();
            self.RemoveAt(self.Count - 1);
            return ret;
        }

        /// <summary>
        /// Alias for <code>self[^1]</code>
        /// </summary>
        /// <returns><code>self[^1]</code></returns>
        public static T Peek<T>(this IList<T> self)
            => self[^1];

        

        /// <summary>
        /// Checks whether the provided collection is empty
        /// </summary>
        /// <param name="self">Collection to be checked for emptyness</param>
        /// <returns><code>true</code> iff the provided collection is empty</returns>
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> self)
            => self.Count <= 0;

        /// <summary>
        /// Concats string representations of all the elements of the provided sequence into one string, segments separated by specified separator string.
        /// </summary>
        /// <param name="self">The sequence whose string representation to get</param>
        /// <param name="separator">String to be put in between of all segments</param>
        /// <returns>String representation of the sequence</returns>
        public static string MakeString<T>(this IEnumerable<T> self, string separator=", ")
        {
            var ret = new StringBuilder();
            using var it = self.GetEnumerator();

            if (it.MoveNext())
            {
                ret.Append(it.Current.ToString());
                while (it.MoveNext()) ret.Append(separator).Append(it.Current.ToString());
            }

            return ret.ToString();
        }




        /// <summary>
        /// Create an enumerable containing the one specified item
        /// </summary>
        /// <typeparam name="T">Type of the item</typeparam>
        /// <param name="self">The item to iterate over</param>
        /// <returns>Sequence of the one provided element</returns>
        public static IEnumerable<T> Enumerate<T>(this T self)
        {
            yield return self;
        }

        /// <summary>
        /// Concats two arrays
        /// </summary>
        /// <typeparam name="T">Type of the result array elements</typeparam>
        /// <param name="self">First array</param>
        /// <param name="other">Second array</param>
        /// <returns>Aggregate array</returns>
        public static T[] Concat<T>(this T[] self, params T[] other)
            => self.Chain(other).ToArray();

        /// <summary>
        /// Chains two sequences into one that iterates first over all items on the first and than in the second
        /// </summary>
        /// <typeparam name="T">Type of the result sequence elements</typeparam>
        /// <param name="self">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <returns>Aggregate sequence</returns>
        public static IEnumerable<T> Chain<T>(this IEnumerable<T> self, IEnumerable<T> second)
        {
            foreach (var t in self) yield return t;
            foreach (var t in second) yield return t;
        }


        /// <summary>
        /// Sequence made by repeating the specified item
        /// </summary>
        /// <typeparam name="T">Type of the sequence elements</typeparam>
        /// <param name="self">Item to repeat</param>
        /// <param name="count">How many times to repeat the item, <code>null</code> for infinite sequence</param>
        /// <returns></returns>
        public static IEnumerable<T> Repeat<T>(this T self, int? count=null)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), $"Count must be non-negative but is {count}");

            static IEnumerable<T> impl(T self, int count)
            {
                while (--count >= 0) yield return self;
            }
            static IEnumerable<T> impl_inf(T self)
            {
                while (true) yield return self;
            }

            return count== null ? impl_inf(self) : impl(self, count.Value);
        }

        /// <summary>
        /// Infinite sequence of numbers
        /// </summary>
        /// <param name="begin">First element of the sequence</param>
        /// <param name="step">Number to be added to nth element to generate (n+1)th element</param>
        /// <returns>Infinite sequence</returns>
        public static IEnumerable<int> InfiniteRange(int begin = 0, int step = 1)
        {
            for(; ;begin += step )
                yield return begin;
        }

        /// <summary>
        /// For convenience purposes - converts a tuple of 2 elements (Key, Value) into a <see cref="KeyValuePair"/>
        /// </summary>
        /// <param name="pair">Pair to be converted to <see cref="KeyValuePair{TKey, TValue}"/></param>
        /// <returns><see cref="KeyValuePair"/> with corresponding key and value</returns>
        public static KeyValuePair<TKey, TValue> AsKV<TKey, TValue>(this (TKey Key, TValue Value) pair)
            => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);


        /// <summary>
        /// Returns an empty readonly list of specified type
        /// </summary>
        /// <returns>An empty readonly list instance</returns>
        public static IReadOnlyList<T> EmptyList<T>() => ImmutableList<T>.Empty;

        /// <summary>
        /// Returns an empty readonly dictionary of specified type
        /// </summary>
        /// <returns>An empty readonly dictionary instance</returns>
        public static IReadOnlyDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>() => ImmutableDictionary<TKey, TValue>.Empty;

        /// <summary>
        /// Computes a good position-sensitive hash code for given sequence of items.
        /// </summary>
        /// <typeparam name="T">Type of the provided sequence elements</typeparam>
        /// <param name="self">Seqence to compute hash code of</param>
        /// <returns>Good position sensitive hash code for the sequence</returns>
        public static int SequenceHashCode<T>(this IEnumerable<T> self)
        {
            int ret = 0;
            if(self != null) foreach (var t in self)
                ret = HashCode.Combine(ret, t.GetHashCode());
            return ret;
        }

        /// <summary>
        /// Checks whether there is an element that is contained in the collection multiple times
        /// </summary>
        /// <typeparam name="T">Type of the collection elements</typeparam>
        /// <param name="self">The collection to be checked for duplicit element</param>
        /// <param name="comparer">Comparer to be used for determining item eqality, <code>EqualityComparer{T}.Default</code> if <code>null</code></param>
        /// <returns></returns>
        public static bool CheckHasDuplicit<T>(this IReadOnlyCollection<T> self, IEqualityComparer<T> comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;

            return self.Count != self.Distinct(comparer).Count();
        }




        /// <summary>
        /// For convenience purposes - <see cref="System.Collections.Generic.IReadOnlyList{T}"/> variant that has <code>GetEnumerator()</code> methods preimplemented as default methods using the indexer property on <code>this</code>
        /// </summary>
        /// <typeparam name="T">Type of the contained elements</typeparam>
        public interface IReadOnlyList_PreimplementedEnumerator<T> : IReadOnlyList<T>
        {
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                for (int t = 0; t < this.Count; ++t)
                    yield return this[t];
            }


            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
