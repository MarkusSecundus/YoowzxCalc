using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    [YCCompilerDecorator]
    class YCCompilerWithCaching<TNumber> : IYCCompiler<TNumber>
    {
        public bool Force { get; }

        private readonly IYCCompiler<TNumber> _base;


        public YCCompilerWithCaching(IYCCompiler<TNumber> baseCompiler, bool force=false)
            => (_base, Force) = (baseCompiler, force);


        [YCCompilerFactory] private static YCCompilerWithCaching<TNumber> _factory(IYCCompiler<TNumber> baseCompiler) => new YCCompilerWithCaching<TNumber>(baseCompiler);

        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            return (Force || toCompile.Annotations.ContainsKey(IYCCompiler<TNumber>.CachingRequestAnnotation))
                ? new YCCompilationResult<TNumber>(YCCompilationResult<TNumber>.GetExpression(product).Autocached(), YCCompilationResult<TNumber>.GetThisFunctionWrapper(product))
                : product
                ;
        }

    }
}
