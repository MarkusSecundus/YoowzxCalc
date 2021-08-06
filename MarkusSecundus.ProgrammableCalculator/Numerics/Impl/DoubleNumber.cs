using MarkusSecundus.ProgrammableCalculator.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.Numerics.Impl
{
    public struct DoubleNumber : INumber<DoubleNumber>
    {
        public DoubleNumber(double value) => Value = value;
        public DoubleNumber(bool value) => Value = value?1:0;

        public readonly double Value;

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
        public DoubleNumber Gt(DoubleNumber other) => Value > other.Value;

        public DoubleNumber Le(DoubleNumber other) => Value <= other.Value;
        public DoubleNumber Ge(DoubleNumber other) => Value >= other.Value;

        public DoubleNumber Eq(DoubleNumber other) => Value == other.Value;
        public DoubleNumber Ne(DoubleNumber other) => Value != other.Value;


        public static implicit operator DoubleNumber(double i) => new DoubleNumber(i);
        public static implicit operator double(DoubleNumber i) => i.Value;
        
        public static implicit operator DoubleNumber(bool b) => new DoubleNumber(b);
        public static explicit operator bool(DoubleNumber i) => !i.IsZero();



        public static DoubleNumber operator +(DoubleNumber a, DoubleNumber b) => a.Value + b.Value;
        public static DoubleNumber operator -(DoubleNumber a, DoubleNumber b) => a.Value - b.Value;
        public static DoubleNumber operator *(DoubleNumber a, DoubleNumber b) => a.Value * b.Value;
        public static DoubleNumber operator /(DoubleNumber a, DoubleNumber b) => a.Value / b.Value;
        public static DoubleNumber operator %(DoubleNumber a, DoubleNumber b) => a.Value % b.Value;
        
        
        public static DoubleNumber operator -(DoubleNumber a) => -a.Value;
        public static DoubleNumber operator ^(DoubleNumber a, DoubleNumber b) => Math.Pow(a.Value, b.Value);
        public static DoubleNumber operator !(DoubleNumber a)=> a.Value == 0;

        public static DoubleNumber operator <(DoubleNumber a, DoubleNumber b) => a.Value < b.Value;
        public static DoubleNumber operator >(DoubleNumber a, DoubleNumber b) => a.Value > b.Value;
        public static DoubleNumber operator <=(DoubleNumber a, DoubleNumber b) => a.Value <= b.Value;
        public static DoubleNumber operator >=(DoubleNumber a, DoubleNumber b) => a.Value >= b.Value;

        public static DoubleNumber operator ==(DoubleNumber a, DoubleNumber b) => a.Value == b.Value;
        public static DoubleNumber operator !=(DoubleNumber a, DoubleNumber b) => a.Value != b.Value;










        public override bool Equals(object obj) => obj is DoubleNumber n && Value == n.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => $"{Value}d";

    }
}
