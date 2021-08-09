using MarkusSecundus.Util;
using System;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IASTCompilationContext<TNumber> : IASTInterpretationContext<TNumber>
    {
        public SettableOnce<Delegate> GetUnresolvedFunction(FunctionSignature<TNumber> signature);
    }
}
