using MarkusSecundus.ProgrammableCalculator.Compiler.Impl;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Contexts
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
