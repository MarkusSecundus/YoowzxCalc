using MarkusSecundus.Util;
using System;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IYCCompilationContext<TNumber> : IYCInterpretationContext<TNumber>
    {
        public SettableOnce<Delegate> GetUnresolvedFunction(YCFunctionSignature<TNumber> signature);
    }
}
