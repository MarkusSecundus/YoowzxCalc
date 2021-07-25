using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public interface INumber<TSelf> where TSelf : INumber<TSelf>
    {
        public TSelf Add(TSelf other);

        public TSelf Sub(TSelf other);

        public TSelf Mul(TSelf other);

        public TSelf Div(TSelf other);

        public TSelf Abs();

        public TSelf Neg();

        public TSelf Pow(TSelf power);


        public bool IsZero { get; }

        public TSelf NegLogical();

        public TSelf Lt(TSelf other);
        public TSelf Le(TSelf other);
        public TSelf Gt(TSelf other) => Le(other).NegLogical();
        public TSelf Ge(TSelf other) => Lt(other).NegLogical();

        public TSelf Eq(TSelf other);
        public TSelf Ne(TSelf other) => Eq(other).NegLogical();

    }
}
