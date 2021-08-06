using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Contexts
{
    public interface IASTInterpretationContext<TNumber>
    {
        public IReadOnlyDictionary<FunctionSignature<TNumber>, Delegate> Functions { get; }
    }
}
