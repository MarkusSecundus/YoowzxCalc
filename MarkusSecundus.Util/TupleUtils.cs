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
        /// <summary>
        /// Array of all <see cref="ValueTuple"/> types ordered by count of their generic parameters, beginning with the zero parametered one
        /// </summary>
        public static readonly Type[] TupleTypesByArgsCount = 
             typeof(ValueTuple<>).Assembly.GetTypes()
            .Where(IsValueTupleType)
            .OrderBy(t => t.FullName)
            .ToArray();

        /// <summary>
        /// Type of the largest <see cref="ValueTuple"/>
        /// </summary>
        public static Type LargestTupleType => TupleTypesByArgsCount[^1];

        /// <summary>
        /// Maximum size that a tuple can have without resorting to use of the Rest slot
        /// </summary>
        public static int MaxNormalTupleSize => TupleTypesByArgsCount.Length - 2;

        /// <summary>
        /// Gets a fully parametrized type corresponding to a tuple containing arguments of provided types
        /// </summary>
        /// <param name="elements">Types of requested tuple elements</param>
        /// <returns>Type of a tuple with requested elements</returns>
        public static Type GetValueTupleType(params Type[] elements)
        {
            var newElements = AsValueTupleTypeParameters(elements);
            return TupleTypesByArgsCount[newElements.Length].MakeGenericType(newElements);
        }

        /// <summary>
        /// Checks if the provided Type is any kind of <see cref="ValueTuple"/> (with any number of type parameters)
        /// </summary>
        /// <param name="self">Type to be checked</param>
        /// <returns>True if the provided type is <see cref="ValueTuple"/></returns>
        public static bool IsValueTupleType(this Type self) => self.Assembly == typeof(ValueTuple).Assembly && self.FullName.StartsWith("System.ValueTuple");




        /// <summary>
        /// For an arbitrarily long list of types to be carried in a tuple obtains a list of types that need to be passed as parametrized to <see cref="ValueTuple"/> to achieve the goal.
        /// </summary>
        /// <param name="elements">List of types to be contained in a tuple.</param>
        /// <returns>List of types that can be passed as parameters to <see cref="ValueTuple"/></returns>
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

        /// <summary>
        /// Gets list of types of all elements of arbitrarily large value tuple.
        /// </summary>
        /// <param name="self">Specific (generic-parametrized) instance of one of <see cref="ValueTuple"/> types</param>
        /// <returns>List of types of tuple elements.</returns>
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
