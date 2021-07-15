using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.Numerics.Impl
{
    public struct DoubleNumber : INumber<DoubleNumber>
    {
        public DoubleNumber(double value) => Value = value;

        public double Value { get; init; }



        public DoubleNumber Abs() => Value >= 0 ? Value : -Value;

        public DoubleNumber Add(DoubleNumber other) => Value + other;

        public DoubleNumber Sub(DoubleNumber other) => Value - other;

        public DoubleNumber Mul(DoubleNumber other) => Value * other;

        public DoubleNumber Div(DoubleNumber other) => Value / other;

        public DoubleNumber Neg() => -Value;

        public DoubleNumber Pow(DoubleNumber power) => Math.Pow(Value, power);



        public static implicit operator DoubleNumber(double i) => new DoubleNumber(i);
        public static implicit operator double(DoubleNumber i) => i.Value;
    }
}
