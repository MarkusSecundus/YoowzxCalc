using MarkusSecundus.YoowzxCalc.Compiler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public static class BasicNumberOperators
    {
        private static readonly Dictionary<Type, object> _table = new();

        public static void Register<TNumber>(Func<INumberOperator<TNumber>> operatorSupplier)
            => _table[typeof(TNumber)] = operatorSupplier;

        public static INumberOperator<TNumber> Get<TNumber>() => ((Func<INumberOperator<TNumber>>)_table[typeof(TNumber)])();

        static BasicNumberOperators()
        {
            Register(() => new Double());
            Register(() => new Decimal());
            Register(() => new Long());
        }

        public static class Const
        {
            public const NumberStyles NonintegerNumberStyle = NumberStyles.Number | NumberStyles.AllowExponent;
            public const NumberStyles IntegerNumberStyle = NumberStyles.Integer;

            public static readonly Regex IdentifierValidator = new Regex(@"^\p{L}[\p{L}\p{N}]*$", RegexOptions.Compiled);
        }

        public static FormatException DefaultIdentifierValidation(string identifier)
            => Const.IdentifierValidator.IsMatch(identifier) ? null : new FormatException($"Invalid identifier: '{identifier}'");


        public class Double : INumberOperator<double>
        {
            public static Double Instance { get; } = new();

            public bool TryParse(string repr, out double value) => double.TryParse(repr, Const.NonintegerNumberStyle, CultureInfo.InvariantCulture, out value);

            public FormatException ValidateIdentifier(string identifier) => DefaultIdentifierValidation(identifier);

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



            public IReadOnlyDictionary<YCFunctionSignature<double>, Delegate> StandardLibrary { get; }
                = new Dictionary<YCFunctionSignature<double>, Delegate>().MakeFunctionsAdder()
                    .Add<Func<double>>("PI", () => Math.PI)
                    .Add<Func<double>>("E", () => Math.E)
                    .Add<Func<double>>("TAU", () => Math.Tau)
                    .Add<Func<double, double>>("abs", Math.Abs)
                    .Add<Func<double, double>>("sin", Math.Sin)
                    .Add<Func<double, double>>("cos", Math.Cos)
                    .Add<Func<double, double>>("tan", Math.Tan)
                    .Add<Func<double, double>>("asin", Math.Asin)
                    .Add<Func<double, double>>("acos", Math.Acos)
                    .Add<Func<double, double>>("atan", Math.Atan)
                    .Add<Func<double, double, double>>("atan", Math.Atan2)
                    .Add<Func<double, double>>("sinh", Math.Sinh)
                    .Add<Func<double, double>>("cosh", Math.Cosh)
                    .Add<Func<double, double>>("tanh", Math.Tanh)
                    .Add<Func<double, double>>("asinh", Math.Asinh)
                    .Add<Func<double, double>>("acosh", Math.Acosh)
                    .Add<Func<double, double>>("atanh", Math.Atanh)
                    .Add<Func<double, double>>("ceil", Math.Ceiling)
                    .Add<Func<double, double>>("floor", Math.Floor)
                    .Add<Func<double, double>>("round", Math.Round)
                    .Add<Func<double, double>>("trunc", Math.Truncate)
                    .Add<Func<double, double, double, double>>("clamp", Math.Clamp)
                    .Add<Func<double, double, double>>("copy_sign", Math.CopySign)
                    .Add<Func<double, double>>("log2_int", x=>Math.ILogB(x))
                    .Add<Func<double, double>>("log", Math.Log)
                    .Add<Func<double, double>>("log10", Math.Log10)
                    .Add<Func<double, double>>("log2", Math.Log2)
                    .Add<Func<double, double, double>>("max", Math.Max)
                    .Add<Func<double, double, double>>("min", Math.Min)
                    .Add<Func<double, double>>("sign", x=>Math.Sign(x))
                    .Value;
        }

        

        

        

        
        
        public class Decimal : INumberOperator<decimal>
        {
            public static Decimal Instance { get; } = new();

            public bool TryParse(string repr, out decimal value) => decimal.TryParse(repr, Const.NonintegerNumberStyle, CultureInfo.InvariantCulture, out value);

            public FormatException ValidateIdentifier(string identifier) => DefaultIdentifierValidation(identifier);

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

        public class Long : INumberOperator<long>
        {
            public static Long Instance { get; } = new();

            public bool TryParse(string repr, out long value) => long.TryParse(repr, Const.IntegerNumberStyle, CultureInfo.InvariantCulture, out value);

            public FormatException ValidateIdentifier(string identifier) => DefaultIdentifierValidation(identifier);

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
