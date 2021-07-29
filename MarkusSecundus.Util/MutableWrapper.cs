using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public sealed class MutableWrapper<T>
    {
        public T Value { get; set; }
    }
}
