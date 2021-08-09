using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler
{

    public interface IASTInterpreter<TNumber>
    {
        public TNumber Interpret(IASTInterpretationContext<TNumber> ctx, DSLFunctionDefinition toInterpret, IEnumerable<TNumber> args);


        public static IASTInterpreter<TNumber> Make(INumberOperator<TNumber> op) => new ASTInterpreter<TNumber>(op);
    }




    public static class ASTInterpreterExtensions
    {
        public static TNumber Interpret<TNumber>(this IASTInterpreter<TNumber> self, IASTInterpretationContext<TNumber> ctx, DSLFunctionDefinition toInterpret, params TNumber[] args)
            => self.Interpret(ctx, toInterpret, args);


        public static IASTCompiler<TNumber> AsCompiler<TNumber>(this IASTInterpreter<TNumber> self)
            => new _CompilerAdapter<TNumber>(self);

        private record _CompilerAdapter<TNumber>(IASTInterpreter<TNumber> Base) : IASTCompiler<TNumber>
        {
            IASTCompilationResult<TNumber> IASTCompiler<TNumber>.Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
            {
                Func<TNumber[], TNumber> ret = args => Base.Interpret(ctx, toCompile, args);
                var args = typeof(TNumber).Repeat(toCompile.Arguments.Count).ToArray();
                return new _CompilationResult(ret.Dearrayize(args));
            }
            private record _CompilationResult(Delegate Value) : IASTCompilationResult<TNumber>
            {
                TDelegate IASTCompilationResult<TNumber>.Compile<TDelegate>() => (TDelegate)Value;
            }
        }



    }
}
