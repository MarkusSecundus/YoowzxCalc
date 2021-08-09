using System;
using System.Collections.Generic;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IYCInterpretationContext<TNumber>
    {
        public IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> Functions { get; }
    }
}
