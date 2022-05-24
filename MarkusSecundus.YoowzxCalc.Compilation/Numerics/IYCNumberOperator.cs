using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using System;
using System.Collections.Generic;

namespace MarkusSecundus.YoowzxCalc.Numerics
{

    /// <summary>
    /// Object providing operations that can be performed on the specified number type by the calculator.
    /// </summary>
    /// <typeparam name="TNumber">Numeric type used to carry computational data.</typeparam>
    public interface IYCNumberOperator<TNumber>
    {
        /// <summary>
        /// Tries to convert provided string to its numeric value.
        /// <para/>
        /// Every string that matches this is considered to be a constant. The ones that do not are then considered identifier-candidate
        /// </summary>
        /// <param name="repr">Text representation of a number.</param>
        /// <param name="value">Result value. Will be set to the respective value if the conversion succeeds or to <c>default(TNumber)</c> if it failed.</param>
        /// <returns>Whether the conversion was successfull - aka. the string is a valid constant representator.</returns>
        public bool TryParseConstant(string repr, out TNumber value);

        /// <summary>
        /// Determines whether the string represents a valid identifier in the calculator grammar.
        /// </summary>
        /// <param name="identifier">Identifier candidate to be validated.</param>
        /// <returns><c>null</c> if the string is a valid identifier or an exception describing errors in its format</returns>
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
        /// <returns>Result of the division</returns>
        public TNumber Divide(TNumber a, TNumber b);


        /// <summary>
        /// Compute a division remainder (<c>'x%y'</c> in C-like languages)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of the modulo</returns>
        public TNumber Modulo(TNumber a, TNumber b);


        /// <summary>
        /// Compute specified power of the first operand (<c>'x**y'</c> in eg. Python)
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Desired power of <c>a</c></returns>
        public TNumber Power(TNumber a, TNumber power);

        /// <summary>
        /// Negate a number arithmetically (<c>'-x'</c> in C-like languages)
        /// </summary>
        /// <param name="a">The operand</param>
        /// <returns>Result of the negation</returns>
        public TNumber UnaryMinus(TNumber a);



        /// <summary>
        /// Get boolean value corresponding to the specified number
        /// </summary>
        /// <param name="a">The operand</param>
        /// <returns>Boolean value corresponding to provided number</returns>
        public bool IsTrue(TNumber a);

        /// <summary>
        /// Get a number whose logical value (<c>this.IsTrue()</c>) is opposite to logical value of the argument number.
        /// </summary>
        /// <param name="a">Number to be negated</param>
        /// <returns>Logical negation</returns>
        public TNumber NegateLogical(TNumber a);

        /// <summary>
        /// Compares the two operands and determines whether the left one is less than the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsLess(TNumber a, TNumber b);

        /// <summary>
        /// Compares the two operands and determines whether the left one is less than or equal to the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsLessOrEqual(TNumber a, TNumber b);

        /// <summary>
        /// Compares the two operands and determines whether the left one is greater than the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsGreater(TNumber a, TNumber b) => NegateLogical(IsGreater(a, b));

        /// <summary>
        /// Compares the two operands and determines whether the left one is greater than or equal to the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsGreaterOrEqual(TNumber a, TNumber b) => NegateLogical(IsLess(a, b));


        /// <summary>
        /// Compares the two operands and determines whether the left one is equal to the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsEqual(TNumber a, TNumber b);

        /// <summary>
        /// Compares the two operands and determines whether the left one is non-equal to the right one.
        /// <para/>
        /// Returns <c>true</c> or <c>false</c> encoded to a <typeparamref name="TNumber"/> such that calling <c>this.IsTrue()</c> on it would yield the corresponding result.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>Bool value encoded to <typeparamref name="TNumber"/></returns>
        public TNumber IsNotEqual(TNumber a, TNumber b) => NegateLogical(IsEqual(a, b));
    }
}
