using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MarkusSecundus.YoowzxCalc.Numerics
{

    /// <summary>
    /// Static class encapsulating basic implementations of <see cref="IYCNumberOperator{TNumber}"/> for the most common numeric types
    /// and providing a basic infrastructure for registering and finding operators by the numeric types they operate on.
    /// </summary>
    public static class YCBasicNumberOperators
    {
        private static readonly Dictionary<Type, object> _table = new();

        /// <summary>
        /// Register an operator provider for given number type.
        /// </summary>
        /// <typeparam name="TNumber">Number type to be operated on</typeparam>
        /// <param name="operatorProvider">Factory creating operator instances for given number type.</param>
        /// <exception cref="InvalidOperationException">If there is already a provider registered for given number type</exception>
        public static void Register<TNumber>(Func<IYCNumberOperator<TNumber>> operatorProvider)
        {
            if (_table.TryGetValue(typeof(TNumber), out var curr))
                throw new InvalidOperationException($"There is already number operator {curr} registered for type {typeof(TNumber)}!");
           _table[typeof(TNumber)] = operatorProvider;
        }

        /// <summary>
        /// Get an instance of <see cref="IYCNumberOperator{TNumber}"/> for given number type
        /// </summary>
        /// <typeparam name="TNumber">Number type to be operated</typeparam>
        /// <returns>An instance of <see cref="IYCNumberOperator{TNumber}"/> registered for given <typeparamref name="TNumber"/> type</returns>
        /// <exception cref="KeyNotFoundException">If there is no operator provider registered for given number type</exception>
        public static IYCNumberOperator<TNumber> Get<TNumber>()
        {
            if (!_table.TryGetValue(typeof(TNumber), out var ret))
                throw new KeyNotFoundException($"No NumberOperator found for the type {typeof(TNumber)}!\n Try adding one by calling Register<{typeof(TNumber)}>(..) or choose one that's available.\n List of number types currently available: [{_table.Keys.MakeString(", ")}]");
            return ((Func<IYCNumberOperator<TNumber>>)ret)();
        }

        static YCBasicNumberOperators()
        {
            Register(() => new Double());
            Register(() => new Decimal());
            Register(() => new Long());
            Register(() => new Dynamic());
        }

        /// <summary>
        /// Subclass containing constant values.
        /// </summary>
        public static class Const
        {
            /// <summary>
            /// Number style describing the common way how number types with fractional part should be parsed
            /// </summary>
            public const NumberStyles NonintegerNumberStyle = NumberStyles.Number | NumberStyles.AllowExponent;

            /// <summary>
            /// Number style describing the common way how integral number types should be parsed
            /// </summary>
            public const NumberStyles IntegerNumberStyle = NumberStyles.Integer;

            /// <summary>
            /// Regex describing the identifier definition that's common in C-like languages
            /// </summary>
            public static readonly Regex IdentifierValidator = new Regex(@"^[_\p{L}][_\p{L}\p{N}]*$", RegexOptions.Compiled);
        }

        /// <summary>
        /// Check whether the identifier matches the specified regex pattern. 
        /// <para/>
        /// If not, create a <see cref="FormatException"/> describing the problem encountered.
        /// </summary>
        /// <param name="identifier">Identifier candidate to be checked for validity</param>
        /// <param name="validator">Pattern that performs the matching. If left default, <see cref="Const.IdentifierValidator"/> will be used</param>
        /// <returns>Null of the candidate matches successfully; the corresponding exception otherwise.</returns>
        public static FormatException ValidateIdentifierFormat(string identifier, Regex validator=null)
            => (validator??Const.IdentifierValidator).IsMatch(identifier) ? null : new FormatException($"Invalid identifier: '{identifier}'");


        /// <summary>
        /// Helper function for parsing string literals. 
        /// Checks if the input represents a string literal (is enclosed in quotes), trims the enclosing quotes and then resolves escape sequences that are inside according to the same specification C# language uses.
        /// </summary>
        /// <param name="repr">Raw literal</param>
        /// <param name="value">Parsed result value</param>
        /// <param name="quoteType">The type of quotes the string is enclosed in - either <c>'</c> or <c>"</c></param>
        /// <returns>If the parse was successful</returns>
        public static bool TryParseQuotedString(string repr, out string value, char quoteType)
        {
            if(repr.Length >= 2 && repr[0] == quoteType && repr[^1] == quoteType)
            {
                value = repr.Substring(1, repr.Length - 2).Replace($"\\{quoteType}", $"{quoteType}").Replace($"{quoteType}{quoteType}", $"{quoteType}");
                return true;
            }
            value = null;
            return false;
        }


        /// <summary>
        /// Basic implementation for calculating on type <see cref="System.Double"/>.
        /// <para/>
        /// Standard library includes all functions and constants from <see cref="System.Math"/>.
        /// </summary>
        public class Double : IYCNumberOperator<double>
        {
            /// <summary>
            /// Instance of the singleton.
            /// </summary>
            public static Double Instance { get; } = new();

            /// <inheritdoc/>
            public bool TryParseConstant(string repr, out double value) => double.TryParse(repr, Const.NonintegerNumberStyle, CultureInfo.InvariantCulture, out value);

            /// <inheritdoc/>
            public FormatException ValidateIdentifier(string identifier) => ValidateIdentifierFormat(identifier);

            private static double toBool(bool d) => d ? 1d : 0d;

            /// <inheritdoc/>
            public double Add(double a, double b) => a + b;
            /// <inheritdoc/>
            public double Subtract(double a, double b) => a - b;
            /// <inheritdoc/>
            public double Multiply(double a, double b) => a * b;
            /// <inheritdoc/>
            public double Divide(double a, double b) => a / b;
            /// <inheritdoc/>
            public double Modulo(double a, double b) => a % b;
            /// <inheritdoc/>
            public double Power(double a, double b) => Math.Pow(a, b);

            /// <inheritdoc/>
            public double IsEqual(double a, double b) => toBool(a == b);
            /// <inheritdoc/>
            public double IsNotEqual(double a, double b) => toBool(a != b);

            /// <inheritdoc/>
            public double IsLessOrEqual(double a, double b) => toBool(a <= b);
            /// <inheritdoc/>
            public double IsLess(double a, double b) => toBool(a < b);
            /// <inheritdoc/>
            public double IsGreaterOrEqual(double a, double b) => toBool(a >= b);
            /// <inheritdoc/>
            public double IsGreater(double a, double b) => toBool(a > b);

            /// <inheritdoc/>
            public bool IsTrue(double a) => a != 0;

            /// <inheritdoc/>
            public double UnaryMinus(double a) => -a;
            /// <inheritdoc/>
            public double NegateLogical(double a) => toBool(a == 0);


            /// <summary>
            /// <inheritdoc/>
            /// Contains all functions from <see cref="System.Math"/>
            /// </summary>
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









        /// <summary>
        /// Basic implementation for calculating on type <see cref="System.Decimal"/>
        /// </summary>
        public class Decimal : IYCNumberOperator<decimal>
        {
            /// <summary>
            /// Instance of the singleton.
            /// </summary>
            public static Decimal Instance { get; } = new();

            public bool TryParseConstant(string repr, out decimal value) => decimal.TryParse(repr, Const.NonintegerNumberStyle, CultureInfo.InvariantCulture, out value);

            public FormatException ValidateIdentifier(string identifier) => ValidateIdentifierFormat(identifier);

            private static decimal toBool(bool d) => d ? 1 : 0;

            /// <inheritdoc/>
            public decimal Add(decimal a, decimal b) => a + b;
            /// <inheritdoc/>
            public decimal Subtract(decimal a, decimal b) => a - b;
            /// <inheritdoc/>
            public decimal Multiply(decimal a, decimal b) => a * b;
            /// <inheritdoc/>
            public decimal Divide(decimal a, decimal b) => a / b;
            /// <inheritdoc/>
            public decimal Modulo(decimal a, decimal b) => a % b;
            /// <inheritdoc/>
            public decimal Power(decimal a, decimal b) => (decimal)Math.Pow((double)a, (double)b);

            /// <inheritdoc/>
            public decimal IsEqual(decimal a, decimal b) => toBool(a == b);
            /// <inheritdoc/>
            public decimal IsNotEqual(decimal a, decimal b) => toBool(a != b);

            /// <inheritdoc/>
            public decimal IsLessOrEqual(decimal a, decimal b) => toBool(a <= b);
            /// <inheritdoc/>
            public decimal IsLess(decimal a, decimal b) => toBool(a < b);
            /// <inheritdoc/>
            public decimal IsGreaterOrEqual(decimal a, decimal b) => toBool(a >= b);
            /// <inheritdoc/>
            public decimal IsGreater(decimal a, decimal b) => toBool(a > b);

            /// <inheritdoc/>
            public bool IsTrue(decimal a) => a != 0;

            /// <inheritdoc/>
            public decimal UnaryMinus(decimal a) => -a;
            /// <inheritdoc/>
            public decimal NegateLogical(decimal a) => toBool(a == 0);
        }

        /// <summary>
        /// Basic implementation for calculating on type <see cref="System.Int64"/>
        /// </summary>
        public class Long : IYCNumberOperator<long>
        {
            /// <summary>
            /// Instance of the singleton.
            /// </summary>
            public static Long Instance { get; } = new();

            /// <inheritdoc/>
            public bool TryParseConstant(string repr, out long value) => long.TryParse(repr, Const.IntegerNumberStyle, CultureInfo.InvariantCulture, out value);

            /// <inheritdoc/>
            public FormatException ValidateIdentifier(string identifier) => ValidateIdentifierFormat(identifier);

            
            private static long toBool(bool d) => d ? 1 : 0;

            /// <inheritdoc/>
            public long Add(long a, long b) => a + b;
            /// <inheritdoc/>
            public long Subtract(long a, long b) => a - b;
            /// <inheritdoc/>
            public long Multiply(long a, long b) => a * b;
            /// <inheritdoc/>
            public long Divide(long a, long b) => a / b;
            /// <inheritdoc/>
            public long Modulo(long a, long b) => a % b;
            /// <inheritdoc/>
            public long Power(long a, long b) => (long)Math.Pow(a, b);

            /// <inheritdoc/>
            public long IsEqual(long a, long b) => toBool(a == b);
            /// <inheritdoc/>
            public long IsNotEqual(long a, long b) => toBool(a != b);

            /// <inheritdoc/>
            public long IsLessOrEqual(long a, long b) => toBool(a <= b);
            /// <inheritdoc/>
            public long IsLess(long a, long b) => toBool(a < b);
            /// <inheritdoc/>
            public long IsGreaterOrEqual(long a, long b) => toBool(a >= b);
            /// <inheritdoc/>
            public long IsGreater(long a, long b) => toBool(a > b);

            /// <inheritdoc/>
            public bool IsTrue(long a) => a != 0;

            /// <inheritdoc/>
            public long UnaryMinus(long a) => -a;
            /// <inheritdoc/>
            public long NegateLogical(long a) => toBool(a == 0);
        }


        /// <summary>
        /// Basic implementation that allows operating on multiple arbitrary types by exploiting the Dynamic Language Runtime.
        /// This comes at the cost of more memory allocations and overall hindered performance.
        /// </summary>
        public class Dynamic : IYCNumberOperator<object>
        {
            /// <summary>
            /// Instance of the singleton.
            /// </summary>
            public static Dynamic Instance { get; } = new();


            /// <summary>
            /// Tries to parse the literal (in this order) as type <see cref="System.Boolean"/>, <see cref="System.Int64"/>, <see cref="System.Double"/> and <see cref="System.String"/> 
            /// (both single- and double-quoted, while resolving escape sequences similarly as C# does)
            /// </summary>
            /// <param name="repr"><inheritdoc/></param>
            /// <param name="value"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public bool TryParseConstant(string repr, out dynamic value)
            {
                value = null;
                if (bool.TryParse(repr, out var boolValue))
                    value = boolValue;
                else if (Long.Instance.TryParseConstant(repr, out var longValue))
                    value = longValue;
                else if (Double.Instance.TryParseConstant(repr, out var doubleValue))
                    value = doubleValue;
                else if (YCBasicNumberOperators.TryParseQuotedString(repr, out var singleQuoteStringValue, '\''))
                    value = singleQuoteStringValue;
                else if (YCBasicNumberOperators.TryParseQuotedString(repr, out var doubleQuoteStringValue, '"'))
                    value = doubleQuoteStringValue;
                else return false;
                return true;
            }

            /// <inheritdoc/>
            public FormatException ValidateIdentifier(string identifier)
                => YCBasicNumberOperators.ValidateIdentifierFormat(identifier);

            /// <inheritdoc/>
            public dynamic Add(dynamic a, dynamic b) => a + b;
            /// <inheritdoc/>
            public dynamic Subtract(dynamic a, dynamic b) => a - b;
            /// <inheritdoc/>
            public dynamic Multiply(dynamic a, dynamic b) => a * b;
            /// <inheritdoc/>
            public dynamic Divide(dynamic a, dynamic b) => a / b;
            /// <inheritdoc/>
            public dynamic Modulo(dynamic a, dynamic b) => a % b;
            /// <inheritdoc/>
            public dynamic Power(dynamic a, dynamic b) => Math.Pow(a, b);

            /// <inheritdoc/>
            public dynamic IsEqual(dynamic a, dynamic b) => a.Equals(b);
            /// <inheritdoc/>
            public dynamic IsNotEqual(dynamic a, dynamic b) => !a.Equals(b);

            /// <inheritdoc/>
            public dynamic IsLessOrEqual(dynamic a, dynamic b) => a <= b;
            /// <inheritdoc/>
            public dynamic IsLess(dynamic a, dynamic b) => a < b;
            /// <inheritdoc/>
            public dynamic IsGreaterOrEqual(dynamic a, dynamic b) => a >= b;
            /// <inheritdoc/>
            public dynamic IsGreater(dynamic a, dynamic b) => a > b;

            /// <inheritdoc/>
            public bool IsTrue(dynamic a) => (bool)a;

            /// <inheritdoc/>
            public dynamic UnaryMinus(dynamic a) => -a;
            /// <inheritdoc/>
            public dynamic NegateLogical(dynamic a) => a == 0;
        }
    }
}
