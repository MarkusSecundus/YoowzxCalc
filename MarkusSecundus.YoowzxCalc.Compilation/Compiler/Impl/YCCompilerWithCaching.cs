using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCCompilerWithCaching<TNumber> : IYCCompiler<TNumber>
    {
        public bool Force { get; }

        private readonly IYCCompiler<TNumber> _base;

        public YCCompilerWithCaching(IYCCompiler<TNumber> baseCompiler, bool force=false)
            => (_base, Force) = (baseCompiler, force);

        public YCCompilationResult<TNumber> Compile(IYCCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            return (Force || toCompile.Annotations.ContainsKey(IYCCompiler<TNumber>.CachingRequestAnnotation))
                ? new YCCompilationResult<TNumber>(YCCompilationResult<TNumber>.GetExpression(product).Autocached(), YCCompilationResult<TNumber>.GetThisFunctionWrapper(product))
                : product
                ;
        }

    }
}
