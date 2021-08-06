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
            .Where(type => type.FullName.StartsWith("System.ValueTuple"))
            .OrderBy(t => t.FullName)
            .ToArray();


        public static Type GetValueTupleType(params Type[] elements)
        {
            int max = TupleTypesByArgsCount.Length-1;
            if (elements.Length <= 0)
                throw new ArgumentException($"Cannot create an empty tuple type!", nameof(elements));
            if(elements.Length < max)
            {
                return TupleTypesByArgsCount[elements.Length].MakeGenericType(elements);
            }
            var condensedElements = elements[..^2].Chain(GetValueTupleType(elements[(max - 1)..]).Enumerate()).ToArray();
            return TupleTypesByArgsCount[max].MakeGenericType(condensedElements);
        }
    }
}
