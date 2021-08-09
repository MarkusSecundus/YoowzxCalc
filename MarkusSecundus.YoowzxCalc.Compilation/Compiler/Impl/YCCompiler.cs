using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCCompiler<TNumber> : IYCCompiler<TNumber>
    {
        private readonly YCCompilerBase<TNumber> Base;

        public YCCompiler(INumberOperator<TNumber> numberOperator)
            => Base = new(numberOperator);

        public IYCCompilationResult<TNumber> Compile(IYCCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            IYCCompiler<TNumber> compiler = Base;
            if (toCompile.Annotations.ContainsKey(IYCCompiler<TNumber>.CachingRequestAnnotation))
                compiler = new YCCompilerWithCaching<TNumber>(compiler);
            
            return compiler.Compile(ctx, toCompile);
        }
    }
}
