using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTCompiler<TNumber> : IASTCompiler<TNumber>
    {
        private readonly INumberOperator<TNumber> Op;

        public ASTCompiler(INumberOperator<TNumber> numberOperator)
            => Op = numberOperator;


        public Delegate Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
        {
            var args = toCompile.Arguments.Select(name => (name, Expression.Parameter(typeof(TNumber), name)).AsKV()).ToImmutableDictionary();

            var compilationContext = new VisitContext
            (
                CoreContext: ctx,
                Father: this,
                ThisFunctionSignature: toCompile.GetSignature<TNumber>(),
                Args: args
            );

            return compilationContext.ThisFunctionWrapper.Value = generateExpression(compilationContext, toCompile).Compile();
        }

        protected virtual LambdaExpression generateExpression(VisitContext ctx, DSLFunctionDefinition toCompile)
        {
            return Expression.Lambda(
                toCompile.Body.Accept(CompilerVisitor.Instance, ctx),
                ctx.Args.Values.ToArray()
            );
        }


        protected record VisitContext
        (
            IASTCompilationContext<TNumber> CoreContext,
            ASTCompiler<TNumber> Father,
            FunctionSignature<TNumber> ThisFunctionSignature,
            ImmutableDictionary<string, ParameterExpression> Args
        )
        {
            public readonly SettableOnce<Delegate> ThisFunctionWrapper = new();

            public Expression OpE { get; }  = Expression.Constant(Father.Op);

            public INumberOperator<TNumber> Op => Father.Op;
        }


        private class CompilerVisitor : DSLVisitorBase<Expression, VisitContext>
        {
            private CompilerVisitor() {}
            public static CompilerVisitor Instance { get; } = new();

            private Expression v(DSLExpression e, VisitContext ctx)
                => e.Accept(this, ctx);

            private static MethodInfo f(Func<TNumber, TNumber, TNumber> binaryOp) => binaryOp.Method;
            private static MethodInfo f(Func<TNumber, TNumber> unaryOp) => unaryOp.Method;
            private static MethodInfo f(Func<TNumber, bool> unaryOp) => unaryOp.Method;

            private Expression visitUnary(DSLUnaryExpression expr, VisitContext ctx, Func<TNumber, TNumber> i)
                => Expression.Call(ctx.OpE, f(i), v(expr.Child, ctx));
            private Expression visitBinary(DSLBinaryExpression expr, VisitContext ctx, Func<TNumber, TNumber, TNumber> i)
                => Expression.Call(ctx.OpE, f(i), v(expr.LeftChild, ctx), v(expr.RightChild, ctx));


            public override Expression Visit(DSLConstantExpression expr, VisitContext ctx)
                => Expression.Constant(ctx.Op.Parse(expr.Value));

            public override Expression Visit(DSLArgumentExpression expr, VisitContext ctx)
                => ctx.Args[expr.ArgumentName];



            public override Expression Visit(DSLUnaryMinusExpression expr, VisitContext ctx)
                => visitUnary(expr, ctx, ctx.Op.UnaryMinus);

            public override Expression Visit(DSLUnaryPlusExpression expr, VisitContext ctx)
                => v(expr.Child, ctx);




            public override Expression Visit(DSLAddExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Add);

            public override Expression Visit(DSLSubtractExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Subtract);

            public override Expression Visit(DSLMultiplyExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Multiply);

            public override Expression Visit(DSLDivideExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Divide);

            public override Expression Visit(DSLModuloExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Modulo);

            public override Expression Visit(DSLExponentialExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Power);









            public override Expression Visit(DSLUnaryLogicalNotExpression expr, VisitContext ctx)
                => visitUnary(expr, ctx, ctx.Op.NegateLogical);

            public override Expression Visit(DSLCompareGreaterOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreaterOrEqual);

            public override Expression Visit(DSLCompareGreaterThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsGreater);

            public override Expression Visit(DSLCompareLessOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLessOrEqual);

            public override Expression Visit(DSLCompareLessThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsLess);

            public override Expression Visit(DSLCompareIsEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsEqual);

            public override Expression Visit(DSLCompareIsNotEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.IsNotEqual);





            public override Expression Visit(DSLLogicalAndExpression expr, VisitContext ctx)
            {
                return shortenedLogicalEval(expr, ctx, isAnd: true);
            }

            public override Expression Visit(DSLLogicalOrExpression expr, VisitContext ctx)
            {
                return shortenedLogicalEval(expr, ctx, isAnd: false);
            }

            public override Expression Visit(DSLConditionalExpression expr, VisitContext ctx)
            {
                var condition = Expression.Call(ctx.OpE, f(ctx.Op.IsTrue), v(expr.Condition, ctx));

                return Expression.Condition(condition, v(expr.IfTrue, ctx), v(expr.IfFalse, ctx));
            }





            public override Expression Visit(DSLFunctioncallExpression expr, VisitContext ctx)
            {
                var function = getFunction(expr.GetSignature<TNumber>(), ctx);

                var args = expr.Arguments.Select(arg => v(arg, ctx));

                return Expression.Invoke(function, args);
            }







            






            private Expression shortenedLogicalEval(DSLBinaryExpression expr, VisitContext ctx, bool isAnd)
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



            private static Expression getFunction(FunctionSignature<TNumber> func, VisitContext ctx)
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
                }
                return wrap.Value != null
                    ? Expression.Constant(wrap)
                    : Expression.Convert(
                        Expression.PropertyOrField(Expression.Constant(wrap), nameof(wrap.Value)),
                        func.GetFuncType()
                      );

            }
        }

    }
}
