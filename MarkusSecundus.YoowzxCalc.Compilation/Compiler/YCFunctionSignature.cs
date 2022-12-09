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
        /// <summary>
        /// Constructs function signature with given name and number of arguments
        /// </summary>
        /// <param name="name">Functino name</param>
        /// <param name="argumentsCount">Count of function arguments</param>
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



        /// <summary>
        /// Determines whether two function signatures are equal (have same name, type and numer of arguments)
        /// </summary>
        /// <param name="s">Function signature to compare</param>
        /// <returns><c>true</c> iff the other function signature has the same name, type and number of arguments</returns>
        public bool Equals(YCFunctionSignature<TNumber> s) => (Name, ArgumentsCount) == (s.Name, s.ArgumentsCount);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is YCFunctionSignature<TNumber> s && Equals(s);

        /// <inheritdoc/>
        public override int GetHashCode() => (Name, ArgumentsCount).GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => $"{Name}<{typeof(TNumber).Name}>({ArgumentsCount})";

        /// <summary>
        /// Shorter text representation without type parameter being mentioned.
        /// </summary>
        /// <returns>Shorter text representation of <c>this</c></returns>
        public string ToStringTypeless() => $"{Name}({ArgumentsCount})";

        /// <summary>
        /// Determines whether two function signatures are equal (have same name, type and numer of arguments)
        /// </summary>
        /// <param name="s">Function signature to compare</param>
        /// <returns><c>true</c> iff the other function signature has the same name, type and number of arguments</returns>
        public static bool operator ==(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => a.Equals(b);

        /// <summary>
        /// Determines whether two function signatures are not equal (have different name, type or numer of arguments)
        /// </summary>
        /// <param name="s">Function signature to compare</param>
        /// <returns><c>true</c> iff the other function signature has different name, type or number of arguments</returns>
        public static bool operator !=(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => !(a == b);
    }
}
