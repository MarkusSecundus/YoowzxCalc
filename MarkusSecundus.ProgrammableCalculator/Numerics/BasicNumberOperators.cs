using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public static class BasicNumberOperators
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

        public readonly struct Decimal : INumberOperator<decimal>
        {
            public static Double Instance => new();

            public decimal Parse(string repr) => decimal.Parse(repr, CultureInfo.InvariantCulture);


            private static decimal toBool(bool d) => d ? 1 : 0;

            public decimal AbsoluteValue(decimal a) => Math.Abs(a);

            public decimal Add(decimal a, decimal b) => a + b;
            public decimal Subtract(decimal a, decimal b) => a - b;
            public decimal Multiply(decimal a, decimal b) => a * b;
            public decimal Divide(decimal a, decimal b) => a / b;
            public decimal Modulo(decimal a, decimal b) => a % b;
            public decimal Power(decimal a, decimal b) => (decimal)Math.Pow((double)a, (double)b);

            public decimal IsEqual(decimal a, decimal b) => toBool(a == b);
            public decimal IsNotEqual(decimal a, decimal b) => toBool(a != b);

            public decimal IsLessOrEqual(decimal a, decimal b) => toBool(a <= b);
            public decimal IsLess(decimal a, decimal b) => toBool(a < b);
            public decimal IsGreaterOrEqual(decimal a, decimal b) => toBool(a >= b);
            public decimal IsGreater(decimal a, decimal b) => toBool(a > b);

            public bool IsTrue(decimal a) => a != 0;

            public decimal UnaryMinus(decimal a) => -a;
            public decimal NegateLogical(decimal a) => toBool(a == 0);
        }

        public readonly struct Long : INumberOperator<long>
        {
            public static Long Instance => new();

            public long Parse(string repr) => long.Parse(repr, CultureInfo.InvariantCulture);


            private static long toBool(bool d) => d ? 1 : 0;

            public long AbsoluteValue(long a) => Math.Abs(a);

            public long Add(long a, long b) => a + b;
            public long Subtract(long a, long b) => a - b;
            public long Multiply(long a, long b) => a * b;
            public long Divide(long a, long b) => a / b;
            public long Modulo(long a, long b) => a % b;
            public long Power(long a, long b) => (long)Math.Pow(a, b);

            public long IsEqual(long a, long b) => toBool(a == b);
            public long IsNotEqual(long a, long b) => toBool(a != b);

            public long IsLessOrEqual(long a, long b) => toBool(a <= b);
            public long IsLess(long a, long b) => toBool(a < b);
            public long IsGreaterOrEqual(long a, long b) => toBool(a >= b);
            public long IsGreater(long a, long b) => toBool(a > b);

            public bool IsTrue(long a) => a != 0;

            public long UnaryMinus(long a) => -a;
            public long NegateLogical(long a) => toBool(a == 0);
        }
    }
}
