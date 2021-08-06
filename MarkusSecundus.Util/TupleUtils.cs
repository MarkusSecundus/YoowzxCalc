using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MarkusSecundus.Util
{
    public static class TupleUtils
    {
        public static readonly Type[] TupleTypesByArgsCount = 
             typeof(ValueTuple<>).Assembly.GetTypes()
            .Where(IsValueTupleType)
            .OrderBy(t => t.FullName)
            .ToArray();

        public static Type LastTupleType => TupleTypesByArgsCount[^1];

        public static Type GetValueTupleType(params Type[] elements)
        {
            elements = AsValueTupleTypeParameters(elements);
            return TupleTypesByArgsCount[elements.Length].MakeGenericType(elements);
        }

        public static bool IsValueTupleType(this Type self) => self.Assembly == typeof(ValueTuple).Assembly && self.FullName.StartsWith("System.ValueTuple");

        public static Type[] AsValueTupleTypeParameters(params Type[] elements)
        {
            int max = TupleTypesByArgsCount.Length - 1;
            if (elements.Length <= 0)
                throw new ArgumentException($"Cannot create an empty tuple type!", nameof(elements));
            if (elements.Length < max /*|| (elements.Length == max && elements[^1].IsValueTupleType())*/)
            {
                return elements;
            }
            return elements[..^2].Chain(GetValueTupleType(elements[(max - 1)..]).Enumerate()).ToArray();
        }
    }
}
