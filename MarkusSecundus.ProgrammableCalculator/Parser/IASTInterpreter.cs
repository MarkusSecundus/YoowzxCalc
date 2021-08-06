using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public interface IASTInterpreter<TNumber>
    {
        public TNumber Interpret(IReadOnlyDictionary<FunctionSignature<TNumber>, Delegate> functions, DSLFunctionDefinition toInterpret, IEnumerable<TNumber> args);
    }
}
