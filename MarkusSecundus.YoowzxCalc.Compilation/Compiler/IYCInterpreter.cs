using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler
{

    public interface IYCInterpreter<TNumber>
    {
        public TNumber Interpret(IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, IEnumerable<TNumber> args);


        public static IYCInterpreter<TNumber> Make(IYCNumberOperator<TNumber> op) => new YCInterpreterBase<TNumber>(op);
    }




    public static class ASTInterpreterExtensions
    {
        public static TNumber Interpret<TNumber>(this IYCInterpreter<TNumber> self, IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, params TNumber[] args)
            => self.Interpret(ctx, toInterpret, args);


        public static IYCCompiler<TNumber> AsCompiler<TNumber>(this IYCInterpreter<TNumber> self)
            => new _CompilerAdapter<TNumber>(self);

        private record _CompilerAdapter<TNumber>(IYCInterpreter<TNumber> Base) : IYCCompiler<TNumber>
        {
            IYCCompilationResult<TNumber> IYCCompiler<TNumber>.Compile(IYCCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
            {
                Func<TNumber[], TNumber> ret = args => Base.Interpret(ctx, toCompile, args);
                var args = typeof(TNumber).Repeat(toCompile.Arguments.Count).ToArray();
                return new _CompilationResult(ret.Dearrayize(args));
            }
            private record _CompilationResult(Delegate Value) : IYCCompilationResult<TNumber>
            {
                TDelegate IYCCompilationResult<TNumber>.Compile<TDelegate>() => (TDelegate)Value;
            }
        }



    }
}
