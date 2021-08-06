using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Contexts
{
    public interface IASTCompilationContext<TNumber> : IASTInterpretationContext<TNumber>
    {
        public Settable<Delegate> GetUnresolvedFunction(FunctionSignature<TNumber> signature);
    }
}
