using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    /// <summary>
    /// Service able to interpret given AST of an epxression
    /// </summary>
    /// <typeparam name="TNumber">Number type operated on</typeparam>
    public interface IYCInterpreter<TNumber>
    {
        /// <summary>
        /// Performs the calculation described by the specified AST.
        /// </summary>
        /// <param name="ctx">Context of the interpretation</param>
        /// <param name="toInterpret">Expression to be interpreted</param>
        /// <param name="args">Expression arguments</param>
        /// <returns>Result of the expression</returns>
        /// <exception cref="System.FormatException">If the AST is not valid</exception>
        /// <exception cref="ArgumentException">On wrong number of arguments; On an attempt to call a symbol not defined in the interpretation context.</exception>
        public TNumber Interpret(IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, IEnumerable<TNumber> args);


        /// <summary>
        /// Creates an instance of cannonical interpreter implementation
        /// </summary>
        /// <param name="op">Number operator to be used by the interpreter</param>
        /// <returns>Instance of the cannonical interpreter implementation</returns>
        public static IYCInterpreter<TNumber> Make(IYCNumberOperator<TNumber> op) => new YCInterpreterBase<TNumber>(op);
    }



    /// <summary>
    /// Static class containing some convenience functions for working with <see cref="IYCInterpreter{TNumber}"/>
    /// </summary>
    public static class ASTInterpreterExtensions
    {
        /// <summary>
        /// Performs the calculation described by the specified AST.
        /// <para/>
        /// Overload added for more convenience when calling manually.
        /// </summary>
        /// <param name="ctx">Context of the interpretation</param>
        /// <param name="toInterpret">Expression to be interpreted</param>
        /// <param name="args">Expression arguments</param>
        /// <returns>Result of the expression</returns>
        /// <exception cref="System.FormatException">If the AST is not valid</exception>
        /// <exception cref="ArgumentException">On attempt to call a symbol not defined in the interpretation context</exception>
        public static TNumber Interpret<TNumber>(this IYCInterpreter<TNumber> self, IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, params TNumber[] args)
            => self.Interpret(ctx, toInterpret, args);

    }
}
