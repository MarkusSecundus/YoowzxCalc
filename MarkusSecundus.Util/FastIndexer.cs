using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkusSecundus.Util
{
    public class FastIndexer<TKey, TValue>
    {

        private FastIndexer(Builder bld, IEnumerable<KeyValuePair<TKey, TValue>> items, int count)
        {
            _bld = bld;

            _values = new TValue[count];
            foreach (var (key, val) in items)
                _values[bld.Get(key)] = val;
        }

        private readonly TValue[] _values;
        private readonly Builder _bld;

        public ref TValue this[int i] => ref _values[i];
        public ref TValue this[TKey key] => ref this[_bld.Get(key)];


        public class Builder
        {
            private readonly Dictionary<TKey, int> _mapping = new();

            public int Get(TKey name) => _mapping[name];

            public int AddIfNotPresent(TKey name)
            {
                if (_mapping.TryGetValue(name, out var ret))
                    return ret;
                return _mapping[name] = _mapping.Count;
            }

            public FastIndexer<TKey, TValue> Build(IReadOnlyCollection<KeyValuePair<TKey, TValue>> items) => new(this, items, _mapping.Count);
        }


        
    }
}
