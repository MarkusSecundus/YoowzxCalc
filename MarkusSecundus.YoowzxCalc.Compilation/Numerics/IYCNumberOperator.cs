using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using System;
using System.Collections.Generic;

namespace MarkusSecundus.YoowzxCalc.Numerics
{

    //TODO: finish documentation!
    /// <summary>
    /// Object providing operations that can be performed on the specified number type by the calculator.
    /// </summary>
    /// <typeparam name="TNumber">Numeric type used to carry computational data.</typeparam>
    public interface IYCNumberOperator<TNumber>
    {
        /// <summary>
        /// Tries to convert provided string to its numeric value.
        /// </summary>
        /// <param name="repr">Text representation of a number.</param>
        /// <param name="value">Result value. Will be set to the respective value if the conversion succeeds or to <c>default(TNumber)</c> if it failed.</param>
        /// <returns>Whether the conversion was successfull - aka. the string is a valid constant representator.</returns>
        public bool TryParse(string repr, out TNumber value);

        /// <summary>
        /// Determines whether the string represents a valid identifier in the calculator grammar.
        /// </summary>
        /// <param name="identifier">Identifier candidate to be validated.</param>
        /// <returns>Whether the string is a valid identifier</returns>
        public FormatException ValidateIdentifier(string identifier);

        /// <summary>
        /// Collection of functions to be available for calling from calculator by default.
        /// </summary>
        public IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> StandardLibrary
            => CollectionsUtils.EmptyDictionary<YCFunctionSignature<TNumber>, Delegate>();

        /// <summary>
        /// Add two numbers (<c>'x+y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of the addition</returns>
        public TNumber Add(TNumber a, TNumber b);

        /// <summary>
        /// Subtract one number from other (<c>'x-y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of the subtraction</returns>
        public TNumber Subtract(TNumber a, TNumber b);

        /// <summary>
        /// Multiply two numbers (<c>'x*y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Product of the two numbers</returns>
        public TNumber Multiply(TNumber a, TNumber b);


        /// <summary>
        /// Divide a number by another (<c>'x/y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of the division</returns
        public TNumber Divide(TNumber a, TNumber b);


        /// <summary>
        /// Compute a division remainder (<c>'x%y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of the modulo</returns
        public TNumber Modulo(TNumber a, TNumber b);


        /// <summary>
        /// Negate a number arithmetically (<c>'-x'</c> in C-like languages)
        /// </summary>
        /// <param name="a">The operand</param>
        /// <returns>Result of the negation</returns
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
