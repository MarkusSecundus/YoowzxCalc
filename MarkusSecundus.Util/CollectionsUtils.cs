using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace MarkusSecundus.Util
{
    public static class CollectionsUtils
    {
        public static IList<T> Push<T>(this IList<T> self, T toPush)
        {
            self.Add(toPush);
            return self;
        }

        public static T Pop<T>(this IList<T> self)
        {
            var ret = self.Peek();
            self.RemoveAt(self.Count - 1);
            return ret;
        }

        public static T Peek<T>(this IList<T> self)
            => self[^1];

        

        public static Func<T1, T2, TRet> Autocached<T1, T2, TRet>(this Func<T1, T2, TRet> f)
        {
            DefaultValDict<(T1, T2), TRet> cache = new DefaultValDict<(T1, T2), TRet>(args => f(args.Item1, args.Item2));
            return (x, y) => cache[(x, y)];
        }


        public static bool IsEmpty<T>(this IReadOnlyCollection<T> self)
            => self.Count <= 0;

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

        public static void ForAll<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var t in self) action(t);
        }
        public static void ForAll<T, TRet>(this IEnumerable<T> self, Func<T, TRet> action)
        {
            foreach (var t in self) action(t);
        }


        public static T[] Fill<T>(this T[] self, Func<int, T> valueSupplier)
        {
            for (int t = 0; t < self.Length; ++t) self[t] = valueSupplier(t);
            return self;
        }
        public static T[] Fill<T>(this T[] self, T value)
            => self.Fill(i => value);


        public static IEnumerable<T> Enumerate<T>(this T self)
        {
            yield return self;
        }

        public static T[] Concat<T>(this T[] self, params T[] other)
            => self.Chain(other).ToArray();

        public static IEnumerable<T> Chain<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            foreach (var t in self) yield return t;
            foreach (var t in other) yield return t;
        }

        public static IEnumerable<T> Repeat<T>(this T self, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), $"Count must be non-negative but is {count}");

            static IEnumerable<T> impl(T self, int count)
            {
                while (--count >= 0)
                    yield return self;
            }
            return impl(self, count);
        }

        public static IEnumerable<int> InfiniteRange(int begin = 0, int step = 1)
        {
            for(; ;begin += step )
                yield return begin;
        }


        public static KeyValuePair<TKey, TValue> AsKV<TKey, TValue>(this (TKey Key, TValue Value) pair)
            => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);


        public static IReadOnlyList<T> EmptyList<T>() => ImmutableList<T>.Empty;
        public static IReadOnlyDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>() => ImmutableDictionary<TKey, TValue>.Empty;

        public static int SequenceHashCode<T>(this IEnumerable<T> self)
        {
            int ret = 0;
            if(self != null) foreach (var t in self)
                ret = HashCode.Combine(ret, t.GetHashCode());
            return ret;
        }


        public static bool CheckHasDuplicit<T>(this IReadOnlyCollection<T> self, IEqualityComparer<T> comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;

            return self.Count != self.Distinct(comparer).Count();
        }





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
