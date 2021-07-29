using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MarkusSecundus.Util
{

    /// <summary>
    /// Wrapper nad libovolným IDictionary, který v případě, že tázaný klíč není přítomný,
    /// pro něj vytvoří hodnotu. Jako defaultdict v Pythonu.
    /// </summary>
    /// <typeparam name="K">
    /// Datový typ klíčů.
    /// </typeparam>
    /// <typeparam name="V">
    /// Datový typ hodnot.
    /// </typeparam>
    public struct DefaultValDict<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V>, ICollection<KeyValuePair<K,V>>, IReadOnlyCollection<KeyValuePair<K,V>>
    {
        /// <summary>
        /// Jediný platný konstruktor
        /// </summary>
        /// <param name="supplier">
        /// Funkce k vytváření defaultních hodnot. Jako parametr přebírá klíč,
        /// pro který je nová hodnota vytvářena.
        /// </param>
        /// <param name="baseDict">
        /// Objekt, kterému bude nová instance <see cref="DefaultValDict{K, V}"/>
        /// sloužit jako proxy. Pokud není určen, tak <see cref="new Dictionary{K,V}()"/>
        /// </param>
        public DefaultValDict(Func<K, V> supplier, IDictionary<K, V> baseDict = null)
        {
            this._supplier = supplier;
            this.Base = baseDict ?? new Dictionary<K, V>();
        }



        /*  Jediný opravdu důležitý řádek na celé této třídě */
        public V this[K key]
        {
            get
            {
                V ret;
                if (Base.TryGetValue(key, out ret))
                    return ret;
                else
                    return Base[key] = _supplier(key);
            }
            set => Base[key] = value;
        }

        /// <summary>
        /// Vnitřní implementace, na kterou tato proxy odkazuje.
        /// </summary>
        public readonly IDictionary<K, V> Base;

        /*funkce pro výchozí hodnotu může brát v úvahu i hodnotu tázaného klíče*/
        private readonly Func<K, V> _supplier;




        //odsud níže je vše jenom boilerplate, přesměrovávající na _base

        public ICollection<K> Keys => Base.Keys;

        public ICollection<V> Values => Base.Values;

        public int Count => Base.Count;

        public bool IsReadOnly => Base.IsReadOnly;

        IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;

        IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

        public void Add(K key, V value) => Base.Add(key, value);

        public void Add(KeyValuePair<K, V> item) => Base.Add(item);

        public void Clear() => Base.Clear();

        public bool Contains(KeyValuePair<K, V> item) => Base.Contains(item);

        public bool ContainsKey(K key) => Base.ContainsKey(key);

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) => Base.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => Base.GetEnumerator();

        public bool Remove(K key) => Base.Remove(key);

        public bool Remove(KeyValuePair<K, V> item) => Base.Remove(item);

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value) => Base.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => Base.GetEnumerator();


        public override bool Equals(object obj) => Base.Equals(obj);

        public override int GetHashCode() => Base.GetHashCode();

        public override string ToString() => "default_dict.." + Base.ToString();

    }
}