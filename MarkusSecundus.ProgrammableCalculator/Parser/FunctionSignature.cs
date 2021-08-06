using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public struct FunctionSignature<TNumber> : IEquatable<FunctionSignature<TNumber>> where TNumber : INumber<TNumber>
    {
        public string Name { get; init; }
        public int ArgsCount { get; init; }


        public bool Equals(FunctionSignature<TNumber> s) => (Name, ArgsCount) == (s.Name, s.ArgsCount);

        public override bool Equals(object obj) => obj is FunctionSignature<TNumber> s && Equals(s);


        public override int GetHashCode() => (Name, ArgsCount).GetHashCode();

        public override string ToString() => $"{Name}<{typeof(TNumber).Name}>({ArgsCount})";
    }
}
