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
    }
}
