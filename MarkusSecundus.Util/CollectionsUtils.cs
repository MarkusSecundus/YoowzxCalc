using System;
using System.Collections.Generic;
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
    }
}
