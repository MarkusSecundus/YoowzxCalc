using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public interface IYCCompiler<TNumber>
    {
        public IYCCompilationResult<TNumber> Compile(IYCCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile);

        public static IYCCompiler<TNumber> Make(INumberOperator<TNumber> op) => new YCCompiler<TNumber>(op);

        public static IYCCompiler<TNumber> Cached(IYCCompiler<TNumber> comp) => new YCCompilerWithCaching<TNumber>(comp);
    }

}
