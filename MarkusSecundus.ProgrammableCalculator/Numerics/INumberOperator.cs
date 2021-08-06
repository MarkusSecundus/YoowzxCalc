using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public static class INumberOperator
    {
        public readonly struct Double : INumberOperator<double>
        {
            public static Double Instance => new();

            public double Parse(string repr) => double.Parse(repr, CultureInfo.InvariantCulture);


            private static double toBool(bool d) => d ? 1d : 0d;

            public double AbsoluteValue(double a) => Math.Abs(a);

            public double Add(double a, double b) => a + b;
            public double Subtract(double a, double b) => a - b;
            public double Multiply(double a, double b) => a * b;
            public double Divide(double a, double b) => a / b;
            public double Modulo(double a, double b) => a % b;
            public double Power(double a, double b) => Math.Pow(a, b);

            public double IsEqual(double a, double b) => toBool(a == b);
            public double IsNotEqual(double a, double b) => toBool(a != b);

            public double IsLessOrEqual(double a, double b) => toBool(a <= b);
            public double IsLess(double a, double b) => toBool(a < b);
            public double IsGreaterOrEqual(double a, double b) => toBool(a >= b);
            public double IsGreater(double a, double b) => toBool(a > b);

            public bool IsTrue(double a) => a != 0;

            public double UnaryMinus(double a) => -a;
            public double NegateLogical(double a) => toBool(a == 0);
        }
    }

    public interface INumberOperator<TNumber>
    {
        public TNumber Parse(string repr);


        public TNumber Add(TNumber a, TNumber b);

        public TNumber Subtract(TNumber a, TNumber b);

        public TNumber Multiply(TNumber a, TNumber b);

        public TNumber Divide(TNumber a, TNumber b);

        public TNumber Modulo(TNumber a, TNumber b);

        public TNumber AbsoluteValue(TNumber a);

        public TNumber UnaryMinus(TNumber a);

        public TNumber Power(TNumber a, TNumber power);


        public bool IsTrue(TNumber a);

        public TNumber NegateLogical(TNumber a);

        public TNumber IsLess(TNumber a, TNumber b);
        public TNumber IsLessOrEqual(TNumber a, TNumber b);
        public TNumber IsGreater(TNumber a, TNumber b) => NegateLogical(IsGreater(a, b));
        public TNumber IsGreaterOrEqual(TNumber a, TNumber b) => NegateLogical(IsLess(a, b));

        public TNumber IsEqual(TNumber a, TNumber b);
        public TNumber IsNotEqual(TNumber a, TNumber b) => NegateLogical(IsEqual(a, b));





    }
}
