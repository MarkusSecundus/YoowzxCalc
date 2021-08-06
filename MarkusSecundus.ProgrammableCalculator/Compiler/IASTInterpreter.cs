using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{

    public interface IASTInterpreter<TNumber>
    {
        public TNumber Interpret(IASTInterpretationContext<TNumber> ctx, DSLFunctionDefinition toInterpret, IEnumerable<TNumber> args);

    }




    public static class ASTInterpreterExtensions
    {
        public static TNumber Interpret<TNumber>(this IASTInterpreter<TNumber> self, IASTInterpretationContext<TNumber> ctx, DSLFunctionDefinition toInterpret, params TNumber[] args)
            => self.Interpret(ctx, toInterpret, args);
    }
}
