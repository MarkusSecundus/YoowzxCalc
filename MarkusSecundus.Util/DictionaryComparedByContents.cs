﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Decorator for <see cref="IReadOnlyDictionary{TKey, TValue}"/> that overrides the <c>Equals()</c> and <c>GetHashCode()</c> methods to compare the individual elements of the dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of keys</typeparam>
    /// <typeparam name="TValue">Type of elements</typeparam>
    public class DictionaryComparedByContents<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// Base dictionary this decorator redirects to
        /// </summary>
        public IReadOnlyDictionary<TKey, TValue> Base;

        /// <summary>
        /// Constructs an instance using provided dictionary as base
        /// </summary>
        /// <param name="baseDictionary">Base dictionary to wrap</param>
        public DictionaryComparedByContents(IReadOnlyDictionary<TKey, TValue> baseDictionary) => Base = baseDictionary;

        /// <inheritdoc/>
        public TValue this[TKey key] => Base[key];

        /// <inheritdoc/>
        public IEnumerable<TKey> Keys => Base.Keys;

        /// <inheritdoc/>
        public IEnumerable<TValue> Values => Base.Values;

        /// <inheritdoc/>
        public int Count => Base.Count;

        /// <inheritdoc/>
        public bool ContainsKey(TKey key) => Base.ContainsKey(key);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Base.GetEnumerator();

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => Base.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is DictionaryComparedByContents<TKey, TValue> other && Base.UnorderedSequenceEqual(other.Base);

        /// <inheritdoc/>
        public override int GetHashCode() => Base.SequenceHashCode(CollectionsUtils.CombineHashCodeCommutative);

        /// <inheritdoc/>
        public override string ToString() => Base.ToString();
    }
}
