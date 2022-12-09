using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.Numerics;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    [YCCompilerChainDecorator]
    class YCCompilerWithCaching<TNumber> : IYCCompiler<TNumber>
    {
        public bool Force { get; init; }
        public IYCCompiler<TNumber> Base { get; }

        public IYCNumberOperator<TNumber> NumberOperator => Base.NumberOperator;

        public YCCompilerWithCaching(IYCCompiler<TNumber> baseCompiler, bool force=false)
            => (Base, Force) = (baseCompiler, force);


        [YCCompilerChainFactory] private static YCCompilerWithCaching<TNumber> _factory(IYCCompiler<TNumber> baseCompiler) => new YCCompilerWithCaching<TNumber>(baseCompiler);

        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var product = Base.Compile(ctx, toCompile);

            return (Force || toCompile.Annotations.ContainsKey(IYCCompiler<TNumber>.CachingRequestAnnotation))
                ? new YCCompilationResult<TNumber>(YCCompilationResult<TNumber>.GetExpression(product).Autocached(), YCCompilationResult<TNumber>.GetThisFunctionWrapper(product))
                : product
                ;
        }

    }
}
