using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public sealed class SettableOnce<T>
    {
        private T _value;

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

        public bool IsSet { get; private set; } = false;



        public override bool Equals(object obj) => obj is SettableOnce<T> w && IsSet == w.IsSet && (!IsSet || Equals(Value, w.Value));

        public override int GetHashCode() => IsSet ? Value.GetHashCode() : 0;

        public override string ToString() => IsSet ? "<nil>" : ""+Value;
    }
}
