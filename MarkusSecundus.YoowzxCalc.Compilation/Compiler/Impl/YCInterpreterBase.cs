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
    class YCInterpreterBase<TNumber> : IYCInterpreter<TNumber>
    {
        private readonly INumberOperator<TNumber> op;

        public YCInterpreterBase(INumberOperator<TNumber> numberOperator)
            => op = numberOperator;

        public TNumber Interpret(IYCInterpretationContext<TNumber> ctx, YCFunctionDefinition toInterpret, IEnumerable<TNumber> args)
        {
            var argsDict = args.Select((arg, index) => (toInterpret.Arguments[index], arg).AsKV()).ToImmutableDictionary();
            if (argsDict.Count != toInterpret.Arguments.Count)
                throw new YCCompilerException($"Interpreting function: `{toInterpret.GetSignature<TNumber>()}` - {argsDict.Count} args provided instead of {toInterpret.Arguments.Count}");

            return toInterpret.Body.Accept(InterpreterVisitor.Instance, new VisitContext
            (
                CoreContext: ctx,
                Father: this,
                ThisFunction: toInterpret,
                Args: argsDict
            ));
        }




        private record VisitContext
        (
            IYCInterpretationContext<TNumber> CoreContext,
            YCInterpreterBase<TNumber> Father,
            YCFunctionDefinition ThisFunction,
            ImmutableDictionary<string, TNumber> Args
        )
        {
            public INumberOperator<TNumber> Op => Father.op;

            public TNumber InterpretRecursively(IEnumerable<TNumber> args)
                => Father.Interpret(CoreContext, ThisFunction, args);
        }

        private class InterpreterVisitor : YCVisitorBase<TNumber, VisitContext>
        {
            private InterpreterVisitor() { }
            public static InterpreterVisitor Instance { get; } = new();



            private TNumber v(YCExpression e, VisitContext ctx)
                => e.Accept(this, ctx);


            public override TNumber Visit(YCLiteralExpression expr, VisitContext ctx)
            {
                if(ctx.Op.TryParse(expr.Value, out var constant))
                    return constant;
                else
                {
                    var formatError = ctx.Op.ValidateIdentifier(expr.Value);
                    if (formatError != null)
                        throw formatError;

                    return ctx.Args.TryGetValue(expr.Value, out var ret)
                        ? ret
                        : v(new YCFunctioncallExpression { Name = expr.Value, Arguments = CollectionsUtils.EmptyList<YCExpression>() }, ctx);
                }
            }



            public override TNumber Visit(YCUnaryMinusExpression expr, VisitContext ctx)
                => ctx.Op.UnaryMinus(v(expr.Child, ctx));

            public override TNumber Visit(YCUnaryPlusExpression expr, VisitContext ctx)
                => v(expr.Child, ctx);




            private TNumber visitBinary(YCBinaryExpression expr, VisitContext ctx, Func<TNumber, TNumber, TNumber> f)
                => f(v(expr.LeftChild, ctx), v(expr.RightChild, ctx));


            public override TNumber Visit(YCAddExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Add);

            public override TNumber Visit(YCSubtractExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Subtract);

            public override TNumber Visit(YCMultiplyExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Multiply);

            public override TNumber Visit(YCDivideExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Divide);

            public override TNumber Visit(YCModuloExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Modulo);

            public override TNumber Visit(YCExponentialExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Power);









            public override TNumber Visit(YCUnaryLogicalNotExpression expr, VisitContext ctx)
                => ctx.Op.NegateLogical(v(expr.Child, ctx));


            public override TNumber Visit(YCCompareGreaterOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreaterOrEqual);

            public override TNumber Visit(YCCompareGreaterThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreater);

            public override TNumber Visit(YCCompareLessOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLessOrEqual);

            public override TNumber Visit(YCCompareLessThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLess);

            public override TNumber Visit(YCCompareIsEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsEqual);

            public override TNumber Visit(YCCompareIsNotEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsNotEqual);





            public override TNumber Visit(YCLogicalAndExpression expr, VisitContext ctx)
            {
                var left = v(expr.LeftChild, ctx);
                return ctx.Op.IsTrue(left) ? v(expr.RightChild, ctx) : left;
            }

            public override TNumber Visit(YCLogicalOrExpression expr, VisitContext ctx)
            {
                var left = v(expr.LeftChild, ctx);
                return ctx.Op.IsTrue(left) ? left : v(expr.RightChild, ctx);
            }

            public override TNumber Visit(YCConditionalExpression expr, VisitContext ctx)
            {
                var condition = v(expr.Condition, ctx);
                return ctx.Op.IsTrue(condition) ? v(expr.IfTrue, ctx) : v(expr.IfFalse, ctx);
            }





            public override TNumber Visit(YCFunctioncallExpression expr, VisitContext ctx)
            {
                var args = expr.Arguments.Select(arg => v(arg, ctx));
                var signature = expr.GetSignature<TNumber>();

                if (signature == ctx.ThisFunction.GetSignature<TNumber>())
                    return ctx.InterpretRecursively(args);

                if (!ctx.CoreContext.Functions.TryGetValue(signature, out var ret))
                    ret = ctx.Op.StandardLibrary[signature];

                return (TNumber) ret.DynamicInvoke(args.Select(e => (object)e).ToArray());
            }
        }
    }
}
