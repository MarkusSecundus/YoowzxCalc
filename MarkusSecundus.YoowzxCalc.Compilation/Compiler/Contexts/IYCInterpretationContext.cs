using System;
using System.Collections.Generic;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{

    /// <summary>
    /// Set of data-carrying contracts sufficient for a context used during interpretation of an YC expression.
    /// </summary>
    /// <typeparam name="TNumber">Number type used by the interpreter</typeparam>
    public interface IYCInterpretationContext<TNumber>
    {
        /// <summary>
        /// Set of functions that can be called by the interpreter
        /// </summary>
        public IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> Functions { get; }
    }
}
