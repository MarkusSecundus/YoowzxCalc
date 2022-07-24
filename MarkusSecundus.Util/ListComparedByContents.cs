using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Decorator for <see cref="IReadOnlyList{T}"/> that overrides the <c>Equals()</c> and <c>GetHashCode()</c> methods to compare the individual elements of the list.
    /// </summary>
    /// <typeparam name="T">Type of elements</typeparam>
    public class ListComparedByContents<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Base list this decorator redirects to
        /// </summary>
        public IReadOnlyList<T> Base;

        /// <summary>
        /// Constructs an instance using provided list as base
        /// </summary>
        /// <param name="baseList">Base list to wrap</param>
        public ListComparedByContents(IReadOnlyList<T> baseList) => Base = baseList;

        /// <inheritdoc/>
        public T this[int index] => Base[index];

        /// <inheritdoc/>
        public int Count => Base.Count;

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Base.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ListComparedByContents<T> l && Enumerable.SequenceEqual(Base, l.Base);

        /// <inheritdoc/>
        public override int GetHashCode() => CollectionsUtils.SequenceHashCode(Base);

        /// <inheritdoc/>
        public override string ToString() => Base.ToString();
    }
}
