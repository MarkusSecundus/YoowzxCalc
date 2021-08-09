using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class ASTInterpreter<TNumber> : IASTInterpreter<TNumber>, IASTCompiler<TNumber>
    {
        private readonly INumberOperator<TNumber> op;

        public ASTInterpreter(INumberOperator<TNumber> numberOperator)
            => op = numberOperator;

        public TNumber Interpret(IASTInterpretationContext<TNumber> ctx, DSLFunctionDefinition toInterpret, IEnumerable<TNumber> args)
        {
            var argsDict = args.Select((arg, index) => (toInterpret.Arguments[index], arg).AsKV()).ToImmutableDictionary();
            if (argsDict.Count != toInterpret.Arguments.Count)
                throw new ASTInterpretationException($"Interpreting function: `{toInterpret.GetSignature<TNumber>()}` - {argsDict.Count} args provided instead of {toInterpret.Arguments.Count}");

            return toInterpret.Body.Accept(InterpreterVisitor.Instance, new VisitContext
            (
                CoreContext: ctx,
                Father: this,
                ThisFunction: toInterpret,
                Args: argsDict
            ));
        }

        IASTCompilationResult<TNumber> IASTCompiler<TNumber>.Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
        {
            Func<TNumber[], TNumber> ret = args => this.Interpret(ctx, toCompile, args);
            var args = typeof(TNumber).Repeat(toCompile.Arguments.Count).ToArray();
            return new _CompilationResult(ret.Dearrayize(args));
        }
        private record _CompilationResult(Delegate Value) : IASTCompilationResult<TNumber>
        {
            TDelegate IASTCompilationResult<TNumber>.Compile<TDelegate>() => (TDelegate)Value;
        }


        private record VisitContext
        (
            IASTInterpretationContext<TNumber> CoreContext,
            ASTInterpreter<TNumber> Father,
            DSLFunctionDefinition ThisFunction,
            ImmutableDictionary<string, TNumber> Args
        )
        {
            public INumberOperator<TNumber> Op => Father.op;

            public TNumber InterpretRecursively(IEnumerable<TNumber> args)
                => Father.Interpret(CoreContext, ThisFunction, args);
        }

        private class InterpreterVisitor : DSLVisitorBase<TNumber, VisitContext>
        {
            private InterpreterVisitor() { }
            public static InterpreterVisitor Instance { get; } = new();



            private TNumber v(DSLExpression e, VisitContext ctx)
                => e.Accept(this, ctx);

            public override TNumber Visit(DSLConstantExpression expr, VisitContext ctx)
                => ctx.Op.Parse(expr.Value);

            public override TNumber Visit(DSLArgumentExpression expr, VisitContext ctx)
                => ctx.Args[expr.ArgumentName];



            public override TNumber Visit(DSLUnaryMinusExpression expr, VisitContext ctx)
                => ctx.Op.UnaryMinus(v(expr.Child, ctx));

            public override TNumber Visit(DSLUnaryPlusExpression expr, VisitContext ctx)
                => v(expr.Child, ctx);




            private TNumber visitBinary(DSLBinaryExpression expr, VisitContext ctx, Func<TNumber, TNumber, TNumber> f)
                => f(v(expr.LeftChild, ctx), v(expr.RightChild, ctx));


            public override TNumber Visit(DSLAddExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Add);

            public override TNumber Visit(DSLSubtractExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Subtract);

            public override TNumber Visit(DSLMultiplyExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Multiply);

            public override TNumber Visit(DSLDivideExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Divide);

            public override TNumber Visit(DSLModuloExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Modulo);

            public override TNumber Visit(DSLExponentialExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Power);









            public override TNumber Visit(DSLUnaryLogicalNotExpression expr, VisitContext ctx)
                => ctx.Op.NegateLogical(v(expr.Child, ctx));


            public override TNumber Visit(DSLCompareGreaterOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreaterOrEqual);

            public override TNumber Visit(DSLCompareGreaterThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreater);

            public override TNumber Visit(DSLCompareLessOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLessOrEqual);

            public override TNumber Visit(DSLCompareLessThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLess);

            public override TNumber Visit(DSLCompareIsEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsEqual);

            public override TNumber Visit(DSLCompareIsNotEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsNotEqual);





            public override TNumber Visit(DSLLogicalAndExpression expr, VisitContext ctx)
            {
                var left = v(expr.LeftChild, ctx);
                return ctx.Op.IsTrue(left) ? v(expr.RightChild, ctx) : left;
            }

            public override TNumber Visit(DSLLogicalOrExpression expr, VisitContext ctx)
            {
                var left = v(expr.LeftChild, ctx);
                return ctx.Op.IsTrue(left) ? left : v(expr.RightChild, ctx);
            }

            public override TNumber Visit(DSLConditionalExpression expr, VisitContext ctx)
            {
                var condition = v(expr.Condition, ctx);
                return ctx.Op.IsTrue(condition) ? v(expr.IfTrue, ctx) : v(expr.IfFalse, ctx);
            }





            public override TNumber Visit(DSLFunctioncallExpression expr, VisitContext ctx)
            {
                var args = expr.Arguments.Select(arg => v(arg, ctx));
                var signature = expr.GetSignature<TNumber>();

                if (signature == ctx.ThisFunction.GetSignature<TNumber>())
                    return ctx.InterpretRecursively(args);

                return (TNumber)ctx.CoreContext.Functions[signature].DynamicInvoke(args.Select(e => (object)e).ToArray());
            }
        }
    }
}
