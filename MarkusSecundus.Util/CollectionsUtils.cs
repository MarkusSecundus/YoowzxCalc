using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

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



        public static string Concat<T>(this IEnumerable<T> self, string separator=", ")
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


        public static IEnumerable<T> Chain<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            foreach (var t in self) yield return t;
            foreach (var t in other) yield return t;
        }



        public static IReadOnlyList<T> EmptyList<T>() => ImmutableList<T>.Empty;

        public static int SequenceHashCode<T>(this IEnumerable<T> self)
        {
            int ret = 0;
            if(self != null) foreach (var t in self)
                ret = HashCode.Combine(ret, t.GetHashCode());
            return ret;
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
