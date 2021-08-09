using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IYCFunctioncallContext<TNumber> : IYCCompilationContext<TNumber>
    {
        public IYCFunctioncallContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<YCFunctionSignature<TNumber>, Delegate>> symbolDefinitions);

        public IEnumerable<YCFunctionSignature<TNumber>> GetUnresolvedSymbolsList();
    }

    public static class IASTFunctioncallContext
    {
        public static IYCFunctioncallContext<TNumber> ResolveSymbols<TNumber>(this IYCFunctioncallContext<TNumber> self, params (YCFunctionSignature<TNumber>, Delegate)[] symbolDefinitions)
            => self.ResolveSymbols(symbolDefinitions.Select(CollectionsUtils.AsKV));


        public static IYCFunctioncallContext<TNumber> Make<TNumber>() => new YCFunctioncallContext<TNumber>();
    }
}
