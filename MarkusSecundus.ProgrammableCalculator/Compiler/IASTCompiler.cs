using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{
    public interface IASTCompiler<TNumber>
    {
        public IASTCompilationResult<TNumber> Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile);
    }

}
