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
    //TODO: dokončit dockomenty!
    /// <summary>
    /// Service able to interpret given AST of an epxression
    /// </summary>
    /// <typeparam name="TNumber"></typeparam>
    public interface IYCInterpreter<TNumber>
    {
        public TNumber Interpret(IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, IEnumerable<TNumber> args);


        public static IYCInterpreter<TNumber> Make(IYCNumberOperator<TNumber> op) => new YCInterpreterBase<TNumber>(op);
    }




    public static class ASTInterpreterExtensions
    {
        public static TNumber Interpret<TNumber>(this IYCInterpreter<TNumber> self, IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, params TNumber[] args)
            => self.Interpret(ctx, toInterpret, args);

    }
}
