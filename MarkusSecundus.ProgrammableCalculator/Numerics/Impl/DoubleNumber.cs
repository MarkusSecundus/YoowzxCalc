using MarkusSecundus.ProgrammableCalculator.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.Numerics.Impl
{
    public struct DoubleNumber : INumber<DoubleNumber>
    {
        public DoubleNumber(double value) => Value = value;
        public DoubleNumber(bool value) => Value = value?1:0;

        public double Value { get; init; }

        public bool IsZero() => Value == 0;

        public DoubleNumber Abs() => Value >= 0 ? Value : -Value;

        public DoubleNumber Add(DoubleNumber other) => Value + other;

        public DoubleNumber Sub(DoubleNumber other) => Value - other;

        public DoubleNumber Mul(DoubleNumber other) => Value * other;

        public DoubleNumber Div(DoubleNumber other) => Value / other;
        
        public DoubleNumber Mod(DoubleNumber other) => Value % other;

        public DoubleNumber Neg() => -Value;

        public DoubleNumber Pow(DoubleNumber power) => Math.Pow(Value, power);


        public DoubleNumber NegLogical() => IsZero();

        public DoubleNumber Lt(DoubleNumber other) => Value < other.Value;

        public DoubleNumber Le(DoubleNumber other) => Value <= other.Value;

        public DoubleNumber Eq(DoubleNumber other) => Value == other.Value;


        public static implicit operator DoubleNumber(double i) => new DoubleNumber(i);
        public static implicit operator double(DoubleNumber i) => i.Value;
        
        public static implicit operator DoubleNumber(bool b) => new DoubleNumber(b);
        public static implicit operator bool(DoubleNumber i) => !i.IsZero();


        public override bool Equals(object obj) => obj is DoubleNumber n && Value == n.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        public class ConstantParser : IConstantParser<DoubleNumber>
        {
            public bool IsValid(string repr)
                => double.TryParse(repr, out var _);

            public DoubleNumber Parse(string repr)
                => double.Parse(repr);
        }
    }
}
