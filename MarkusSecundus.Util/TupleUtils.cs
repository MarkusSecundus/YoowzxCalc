using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MarkusSecundus.Util
{
    /// <summary>
    /// Static class with utility functions that should have been in standard library.
    /// </summary>
    public static class TupleUtils
    {
        public static readonly Type[] TupleTypesByArgsCount = 
             typeof(ValueTuple<>).Assembly.GetTypes()
            .Where(IsValueTupleType)
            .OrderBy(t => t.FullName)
            .ToArray();

        public static Type LastTupleType => TupleTypesByArgsCount[^1];

        public static int MaxNormalTupleSize => TupleTypesByArgsCount.Length - 2;

        public static Type GetValueTupleType(params Type[] elements)
        {
            var newElements = AsValueTupleTypeParameters(elements);
            return TupleTypesByArgsCount[newElements.Length].MakeGenericType(newElements);
        }

        public static bool IsValueTupleType(this Type self) => self.Assembly == typeof(ValueTuple).Assembly && self.FullName.StartsWith("System.ValueTuple");

        public static Type[] AsValueTupleTypeParameters(params Type[] elements)
        {
            int max = MaxNormalTupleSize;
            if (elements.Length <= 0)
                throw new ArgumentException($"Cannot create an empty tuple type!", nameof(elements));
            if (elements.Length <= max /*|| (elements.Length == max && elements[^1].IsValueTupleType())*/)
            {
                return elements;
            }
            return elements[..max].Concat(GetValueTupleType(elements[(max)..]));
        }

        public static Type[] GetValueTupleElementTypes(this Type self)
        {
            if (!self.IsValueTupleType())
                throw new ArgumentException($"Must be ValueTuple", nameof(self));

            var raw = self.GetGenericArguments();
            if (raw.Length <= MaxNormalTupleSize)
                return raw;
            else
                return raw[..^1].Concat(GetValueTupleElementTypes(raw[^1]));
        }
    }
}
