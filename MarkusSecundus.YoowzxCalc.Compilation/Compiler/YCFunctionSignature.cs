using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{

    /// <summary>
    /// Condensed info about function header that serves as identifier for functions in YC.
    /// <para/>
    /// Consists of function name and number of arguments. All arguments and return value are assumed to be of type <typeparamref name="TNumber"/>, argument names are of no significance.
    /// </summary>
    /// <typeparam name="TNumber">Type of the function arguments and return value</typeparam>
    public struct YCFunctionSignature<TNumber> : IEquatable<YCFunctionSignature<TNumber>>
    {
        public YCFunctionSignature(string name, int argumentsCount)
            => (Name, ArgumentsCount) = (name, argumentsCount);

        /// <summary>
        /// Function name
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Count of function arguments. All of them are assumed to be of type <typeparamref name="TNumber"/>.
        /// </summary>
        public int ArgumentsCount { get; init; }




        public bool Equals(YCFunctionSignature<TNumber> s) => (Name, ArgumentsCount) == (s.Name, s.ArgumentsCount);

        public override bool Equals(object obj) => obj is YCFunctionSignature<TNumber> s && Equals(s);

        public override int GetHashCode() => (Name, ArgumentsCount).GetHashCode();

        public override string ToString() => $"{Name}<{typeof(TNumber).Name}>({ArgumentsCount})";

        /// <summary>
        /// Shorter text representation without type parameter being mentioned.
        /// </summary>
        /// <returns>Shorter text representation of <c>this</c></returns>
        public string ToStringTypeless() => $"{Name}({ArgumentsCount})";

        public static bool operator ==(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => a.Equals(b);
        public static bool operator !=(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => !(a == b);
    }
}
