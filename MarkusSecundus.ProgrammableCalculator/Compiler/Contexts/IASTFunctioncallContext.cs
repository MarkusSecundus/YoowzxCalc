using MarkusSecundus.ProgrammableCalculator.Compiler.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Contexts
{
    public interface IASTFunctioncallContext<TNumber> : IASTCompilationContext<TNumber>
    {
        public IASTFunctioncallContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<FunctionSignature<TNumber>, Delegate>> symbolsToBeResolved);


    }

    public static class IASTFunctioncallContext
    {
        public static IASTFunctioncallContext<TNumber> Make<TNumber>() => new ASTFunctioncallContext<TNumber>();
    }
}
