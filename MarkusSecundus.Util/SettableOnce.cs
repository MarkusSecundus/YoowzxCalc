using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Proxy pointing to a value that can be written once and read arbitrarily.
    /// </summary>
    /// <typeparam name="T">
    /// Type of contained value
    /// </typeparam>
    public sealed class SettableOnce<T>
    {
        private T _value;

        /// <summary>
        /// Value this proxy points to
        /// </summary>
        public T Value 
        {
            get => _value;
            set
            {
                if (IsSet)
                    throw new InvalidOperationException($"Value was already set to {_value}");
                _value = value;
                IsSet = true;
            } 
        }

        /// <summary>
        /// Whether the value has already been set
        /// </summary>
        public bool IsSet { get; private set; } = false;



        public override bool Equals(object obj) => obj is SettableOnce<T> w && IsSet == w.IsSet && (!IsSet || Equals(Value, w.Value));

        public override int GetHashCode() => IsSet ? Value.GetHashCode() : 0;

        public override string ToString() => IsSet ? "<nil>" : ""+Value;
    }
}
