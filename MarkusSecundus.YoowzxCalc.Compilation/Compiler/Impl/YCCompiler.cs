using MarkusSecundus.YoowzxCalc.Numerics;
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

        private readonly IYCCompiler<TNumber> Decorated;
        public IYCNumberOperator<TNumber> NumberOperator => Base.NumberOperator;

        public YCCompiler(IYCNumberOperator<TNumber> numberOperator)
        {
            Base = new(numberOperator);
            Decorated = new YCCompilerWithCaching<TNumber>(Base);
        }


        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            if (toCompile.Annotations.TryGetValue("debug_print", out var message))
                Console.WriteLine($"Compiler: {message}");
            
            return Decorated.Compile(ctx, toCompile);
        }
    }
}
