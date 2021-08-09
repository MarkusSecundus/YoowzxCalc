using System;
using System.Collections.Generic;

namespace MarkusSecundus.YoowzxCalc.Compiler.Contexts
{
    public interface IASTInterpretationContext<TNumber>
    {
        public IReadOnlyDictionary<FunctionSignature<TNumber>, Delegate> Functions { get; }
    }
}
