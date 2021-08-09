using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public struct YCFunctionSignature<TNumber> : IEquatable<YCFunctionSignature<TNumber>>
    {
        public YCFunctionSignature(string name, int argumentsCount)
            => (Name, ArgumentsCount) = (name, argumentsCount);

        public string Name { get; init; }
        public int ArgumentsCount { get; init; }




        public bool Equals(YCFunctionSignature<TNumber> s) => (Name, ArgumentsCount) == (s.Name, s.ArgumentsCount);

        public override bool Equals(object obj) => obj is YCFunctionSignature<TNumber> s && Equals(s);

        public override int GetHashCode() => (Name, ArgumentsCount).GetHashCode();

        public override string ToString() => $"{Name}<{typeof(TNumber).Name}>({ArgumentsCount})";


        public static bool operator ==(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => a.Equals(b);
        public static bool operator !=(YCFunctionSignature<TNumber> a, YCFunctionSignature<TNumber> b) => !(a == b);
    }
}
