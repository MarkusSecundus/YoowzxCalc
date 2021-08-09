using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public interface IASTCompiler<TNumber>
    {
        public IASTCompilationResult<TNumber> Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile);

        public static IASTCompiler<TNumber> Make(INumberOperator<TNumber> op) => new ASTCompiler<TNumber>(op);

        public static IASTCompiler<TNumber> Cached(IASTCompiler<TNumber> comp) => new ASTCompilerWithCaching<TNumber>(comp);
    }

}
