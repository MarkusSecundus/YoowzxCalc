using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    [YCCompilerChainBase]
    class YCCompilerBase<TNumber> : IYCCompiler<TNumber>
    {
        public IYCNumberOperator<TNumber> NumberOperator { get;}

        public YCCompilerBase(IYCNumberOperator<TNumber> numberOperator)
            => NumberOperator = numberOperator;


        [YCCompilerChainFactory] private static YCCompilerBase<TNumber> __factory(IYCNumberOperator<TNumber> numberOperator) => new(numberOperator);

        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var args = toCompile.Arguments.Select(name => (name, Expression.Parameter(typeof(TNumber), name)).AsKV()).ToArray();

            YCIdentifierValidator<TNumber>.Instance.Validate(toCompile, NumberOperator);

            var compilationContext = new VisitContext
            (
                CoreContext: ctx,
                Father: this,
                ThisFunctionSignature: toCompile.GetSignature<TNumber>(),
                Args: args.ToImmutableDictionary()
            );


            var ret = Expression.Lambda(
                CompilerVisitor.Instance.v(toCompile.Body, compilationContext),
                tailCall : true,
                args.Select(a => a.Value).ToArray()
            );


            return new YCCompilationResult<TNumber>(ret.Compile(), compilationContext.ThisFunctionWrapper);
        }



        private record VisitContext
        (
            IYCReadOnlyCompilationContext<TNumber> CoreContext,
            YCCompilerBase<TNumber> Father,
            YCFunctionSignature<TNumber> ThisFunctionSignature,
            ImmutableDictionary<string, ParameterExpression> Args
        )
        {
            public readonly SettableOnce<Delegate> ThisFunctionWrapper = new();

            public Expression OpE { get; } = Expression.Constant(Father.NumberOperator);

            public IYCNumberOperator<TNumber> Op => Father.NumberOperator;
        }




        private class CompilerVisitor : YCVisitorBase<Expression, VisitContext>
        {
            private CompilerVisitor() { }
            public static CompilerVisitor Instance { get; } = new();

            internal Expression v(YCExpression e, VisitContext ctx)
                => Expression.Convert(e.Accept(this, ctx), typeof(TNumber));

            private static Expression _DefaultExpression = Expression.Default(typeof(TNumber));

            private static MethodInfo f(Func<TNumber, TNumber, TNumber> binaryOp) => binaryOp.Method;
            private static MethodInfo f(Func<TNumber, TNumber> unaryOp) => unaryOp.Method;
            private static MethodInfo f(Func<TNumber, bool> unaryOp) => unaryOp.Method;

            private Expression visitUnary(YCUnaryExpression expr, VisitContext ctx, Func<TNumber, TNumber> i)
                => Expression.Call(ctx.OpE, f(i), v(expr.Child, ctx));
            private Expression visitBinary(YCBinaryExpression expr, VisitContext ctx, Func<TNumber, TNumber, TNumber> i)
                => Expression.Call(ctx.OpE, f(i), v(expr.LeftChild, ctx), v(expr.RightChild, ctx));



            public override Expression Visit(YCLiteralExpression expr, VisitContext ctx)
            {

                if (ctx.Op.TryParseConstant(expr.Value, out var constant))
                    return Expression.Constant(constant);
                else
                {
                    return ctx.Args.TryGetValue(expr.Value, out var ret)
                        ? ret
                        : v(new YCFunctioncallExpression { Name = expr.Value, Arguments = CollectionsUtils.EmptyList<YCExpression>() }, ctx);
                }
            }



            public override Expression Visit(YCUnaryMinusExpression expr, VisitContext ctx)
                => visitUnary(expr, ctx, ctx.Op.UnaryMinus);

            public override Expression Visit(YCUnaryPlusExpression expr, VisitContext ctx)
                => visitUnary(expr, ctx, ctx.Op.UnaryPlus);




            public override Expression Visit(YCAddExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Add);

            public override Expression Visit(YCSubtractExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Subtract);

            public override Expression Visit(YCMultiplyExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Multiply);

            public override Expression Visit(YCDivideExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Divide);

            public override Expression Visit(YCModuloExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Modulo);

            public override Expression Visit(YCExponentialExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Power);









            public override Expression Visit(YCUnaryLogicalNotExpression expr, VisitContext ctx)
                => visitUnary(expr, ctx, ctx.Op.NegateLogical);

            public override Expression Visit(YCCompareGreaterOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreaterOrEqual);

            public override Expression Visit(YCCompareGreaterThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreater);

            public override Expression Visit(YCCompareLessOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLessOrEqual);

            public override Expression Visit(YCCompareLessThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLess);

            public override Expression Visit(YCCompareIsEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsEqual);

            public override Expression Visit(YCCompareIsNotEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsNotEqual);





            public override Expression Visit(YCLogicalAndExpression expr, VisitContext ctx)
            {
                return shortenedLogicalEval(expr, ctx, isAnd: true);
            }

            public override Expression Visit(YCLogicalOrExpression expr, VisitContext ctx)
            {
                return shortenedLogicalEval(expr, ctx, isAnd: false);
            }

            public override Expression Visit(YCConditionalExpression expr, VisitContext ctx)
            {
                var condition = Expression.Call(ctx.OpE, f(ctx.Op.IsTrue), v(expr.Condition, ctx));

                return Expression.Condition(condition, v(expr.IfTrue, ctx), v(expr.IfFalse, ctx));
            }





            public override Expression Visit(YCFunctioncallExpression expr, VisitContext ctx)
            {
                var signature = expr.GetSignature<TNumber>();

                var function = getFunction(signature, ctx, () => throw new ArgumentException($"Function {signature.ToStringTypeless()} not found!"));

                var args = expr.Arguments.Select(arg => v(arg, ctx));


                return Expression.Invoke(function, args);
            }














            private Expression shortenedLogicalEval(YCBinaryExpression expr, VisitContext ctx, bool isAnd)
            {
                var temp = Expression.Parameter(typeof(TNumber));

                var condition = v(expr.LeftChild, ctx);
                var left = condition;
                var right = v(expr.RightChild, ctx);

                if (isAnd) (left, right) = (right, left);

                return Expression.Block
                (
                    new ParameterExpression[] { temp },
                    Expression.Condition(
                        Expression.Call(ctx.OpE, f(ctx.Op.IsTrue), Expression.Assign(temp, condition)),
                        left,
                        right
                    )
                );
            }



            private static Expression getFunction(YCFunctionSignature<TNumber> func, VisitContext ctx, Action onNull)
            {
                SettableOnce<Delegate> wrap;
                if (func == ctx.ThisFunctionSignature)
                {
                    wrap = ctx.ThisFunctionWrapper;
                }
                else if (ctx.CoreContext.Functions.TryGetValue(func, out var ret))
                {
                    return Expression.Constant(ret);
                }
                else
                {
                    wrap = ctx.CoreContext.GetUnresolvedFunction(func);
                    if (wrap.Value == null && ctx.Op.StandardLibrary.TryGetValue(func, out var std))
                    {
                        return Expression.Constant(std);
                    }
                }

                return wrap.Value != null
                    ? Expression.Constant(wrap.Value)
                    : ExpressionUtils.AssertNotNull(
                          Expression.Convert(
                             Expression.PropertyOrField(Expression.Constant(wrap), nameof(wrap.Value)),
                             func.GetFuncType()
                          ),
                          onNull
                      );

            }
        }

    }
}
