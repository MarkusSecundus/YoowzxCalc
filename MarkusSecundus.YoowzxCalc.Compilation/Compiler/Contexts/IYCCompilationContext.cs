using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{

    /// <summary>
    /// Object carrying definitions of functions that can be called from an YC expression and taking care of tracking not-already-resolved functions attempted to be called from somewhere.
    /// </summary>
    /// <typeparam name="TNumber">Number type used</typeparam>
    public interface IYCCompilationContext<TNumber> : IYCReadOnlyCompilationContext<TNumber>
    {
        /// <summary>
        /// Resolves provided symbols and returns a new instance where those symbols are available from the <c>Functions</c> property.
        /// </summary>
        /// <param name="symbolDefinitions">List of symbols and their definitions</param>
        /// <returns>New instance with newly resolved symbols added to functions list</returns>
        public IYCCompilationContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<YCFunctionSignature<TNumber>, Delegate>> symbolDefinitions);


        /// <summary>
        /// Gets list of symbols that were requested at some point in time and are still not resolved.
        /// </summary>
        /// <returns>List of unresolved symbols</returns>
        public IEnumerable<YCFunctionSignature<TNumber>> GetUnresolvedSymbolsList();


        /// <summary>
        /// Creates an instance of cannonical <see cref="IYCCompilationContext{TNumber}"/> implementation
        /// </summary>
        /// <returns>Instance of the cannonical implementation</returns>
        public static IYCCompilationContext<TNumber> Make() => new YCCompilationContext<TNumber>();
    }



    /// <summary>
    /// Static class containing some convenience functions for working with <see cref="IYCCompilationContext{TNumber}"/>
    /// </summary>
    public static class YCCompilationContextExtensions
    {
        /// <summary>
        /// Resolves provided symbols and returns a new instance where those symbols are available from the <c>Functions</c> property.
        /// </summary>
        /// <param name="symbolDefinitions">List of symbols and their definitions</param>
        /// <returns>New instance with newly resolved symbols added to functions list</returns>
        public static IYCCompilationContext<TNumber> ResolveSymbols<TNumber>(this IYCCompilationContext<TNumber> self, params (YCFunctionSignature<TNumber>, Delegate)[] symbolDefinitions)
            => self.ResolveSymbols(symbolDefinitions.Select(CollectionsUtils.AsKV));
    }
}
