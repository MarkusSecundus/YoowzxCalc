using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IASTFunctioncallContext<TNumber> : IASTCompilationContext<TNumber>
    {
        public IASTFunctioncallContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<FunctionSignature<TNumber>, Delegate>> symbolDefinitions);

        public IEnumerable<FunctionSignature<TNumber>> GetUnresolvedSymbolsList();
    }

    public static class IASTFunctioncallContext
    {
        public static IASTFunctioncallContext<TNumber> ResolveSymbols<TNumber>(this IASTFunctioncallContext<TNumber> self, params (FunctionSignature<TNumber>, Delegate)[] symbolDefinitions)
            => self.ResolveSymbols(symbolDefinitions.Select(CollectionsUtils.AsKV));


        public static IASTFunctioncallContext<TNumber> Make<TNumber>() => new ASTFunctioncallContext<TNumber>();
    }
}
