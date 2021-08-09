using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class ASTCompilerWithCaching<TNumber> : IASTCompiler<TNumber>
    {
        private readonly IASTCompiler<TNumber> _base;

        public ASTCompilerWithCaching(IASTCompiler<TNumber> baseCompiler)
            => _base = baseCompiler;

        public IASTCompilationResult<TNumber> Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            if (product is not ASTCompilationResult<TNumber> ret)
                return product;

            return new ASTCompilationResult<TNumber>(ret.Expression.Autocached(), ret.ThisFunctionWrapper);
        }

    }
}
