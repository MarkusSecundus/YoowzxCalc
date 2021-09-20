﻿using MarkusSecundus.Util;
using System;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{

    /// <summary>
    /// Set of data-carrying contracts sufficient for a context used during compilation of an YC expression.
    /// </summary>
    /// <typeparam name="TNumber">Number type used</typeparam>
    public interface IYCCompilationContext<TNumber> : IYCInterpretationContext<TNumber>
    {
        /// <summary>
        /// Gets a wrapper whose value may already be or hopefully will be in the future set to a function corresponding to the specified signature.
        /// </summary>
        /// <param name="signature">Signature of the sought function</param>
        /// <returns>Wrapper whose value either is or is'nt already set</returns>
        public SettableOnce<Delegate> GetUnresolvedFunction(YCFunctionSignature<TNumber> signature);
    }
}
