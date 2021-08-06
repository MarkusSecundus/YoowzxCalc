using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{
    public struct FunctionSignature<TNumber> : IEquatable<FunctionSignature<TNumber>>
    {
        public string Name { get; init; }
        public int ArgumentsCount { get; init; }




        public bool Equals(FunctionSignature<TNumber> s) => (Name, ArgumentsCount) == (s.Name, s.ArgumentsCount);

        public override bool Equals(object obj) => obj is FunctionSignature<TNumber> s && Equals(s);

        public override int GetHashCode() => (Name, ArgumentsCount).GetHashCode();

        public override string ToString() => $"{Name}<{typeof(TNumber).Name}>({ArgumentsCount})";


        public static bool operator ==(FunctionSignature<TNumber> a, FunctionSignature<TNumber> b) => a.Equals(b);
        public static bool operator !=(FunctionSignature<TNumber> a, FunctionSignature<TNumber> b) => !(a == b);
    }
}
