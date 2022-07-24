using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MarkusSecundus.Util
{

    /// <summary>
    /// Decorator for IDictionary. Adds the functionality of creating default value when none is present but is being asked for.
    /// Similar to `defaultdict` in Python standard library.
    /// </summary>
    /// <typeparam name="K">
    /// Key datatype
    /// </typeparam>
    /// <typeparam name="V">
    /// Value datatype
    /// </typeparam>
    public struct DefaultValDict<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V>, ICollection<KeyValuePair<K,V>>, IReadOnlyCollection<KeyValuePair<K,V>>
    {
        /// <summary>
        /// The only valid constructor (unlike the default one)
        /// </summary>
        /// <param name="supplier">
        /// Supplier of default values. Gets passed the key for which the value is being asked.
        /// </param>
        /// <param name="baseDict">
        /// Object, to whom the new instance of <see cref="DefaultValDict{K, V}"/> will serve as decorator (by default <c>new <see cref="Dictionary{K,V}()"/></c> )
        /// </param>
        public DefaultValDict(Func<K, V> supplier, IDictionary<K, V> baseDict = null)
        {
            this._supplier = supplier;
            this.Base = baseDict ?? new Dictionary<K, V>();
        }


        /*  The only really important piece of code in the whole of this class xD */
        /// <inheritdoc/>
        public V this[K key]
        {
            get
            {
                if (Base.TryGetValue(key, out V ret))
                    return ret;
                else
                    return Base[key] = _supplier(key);
            }
            set => Base[key] = value;
        }

        /// <summary>
        /// Inner implementation being pointed to by this decorator.
        /// </summary>
        public readonly IDictionary<K, V> Base;

        /*funkce pro výchozí hodnotu může brát v úvahu i hodnotu tázaného klíče*/
        private readonly Func<K, V> _supplier;




        //odsud níže je vše jenom boilerplate, přesměrovávající na _base

        /// <inheritdoc/>
        public ICollection<K> Keys => Base.Keys;

        /// <inheritdoc/>
        public ICollection<V> Values => Base.Values;

        /// <inheritdoc/>
        public int Count => Base.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => Base.IsReadOnly;

        IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;

        IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

        /// <inheritdoc/>
        public void Add(K key, V value) => Base.Add(key, value);

        /// <inheritdoc/>
        public void Add(KeyValuePair<K, V> item) => Base.Add(item);

        /// <inheritdoc/>
        public void Clear() => Base.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<K, V> item) => Base.Contains(item);

        /// <inheritdoc/>
        public bool ContainsKey(K key) => Base.ContainsKey(key);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) => Base.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => Base.GetEnumerator();

        /// <inheritdoc/>
        public bool Remove(K key) => Base.Remove(key);

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<K, V> item) => Base.Remove(item);

        /// <inheritdoc/>
        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value) => Base.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => Base.GetEnumerator();


        /// <inheritdoc/>
        public override bool Equals(object obj) => Base.Equals(obj);

        /// <inheritdoc/>
        public override int GetHashCode() => Base.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => "default_dict.." + Base.ToString();

    }
}