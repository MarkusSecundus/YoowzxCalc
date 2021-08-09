using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCCompilerWithCaching<TNumber> : IYCCompiler<TNumber>
    {
        private readonly IYCCompiler<TNumber> _base;

        public YCCompilerWithCaching(IYCCompiler<TNumber> baseCompiler)
            => _base = baseCompiler;

        public IYCCompilationResult<TNumber> Compile(IYCCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            if (product is not YCCompilationResult<TNumber> ret)
                return product;

            return new YCCompilationResult<TNumber>(ret.Expression.Autocached(), ret.ThisFunctionWrapper);
        }

    }
}
