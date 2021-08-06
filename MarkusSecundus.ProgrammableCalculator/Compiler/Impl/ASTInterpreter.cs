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
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTInterpreter<TNumber> : IASTInterpreter<TNumber>
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
                => ctx.Op.Neg(v(expr.Child, ctx));

            public override TNumber Visit(DSLUnaryPlusExpression expr, VisitContext ctx)
                => v(expr.Child, ctx);




            private TNumber visitBinary(DSLBinaryExpression expr, VisitContext ctx, Func<TNumber, TNumber, TNumber> f)
                => f(v(expr.LeftChild, ctx), v(expr.RightChild, ctx));


            public override TNumber Visit(DSLAddExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Add);

            public override TNumber Visit(DSLSubtractExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Sub);

            public override TNumber Visit(DSLMultiplyExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Mul);

            public override TNumber Visit(DSLDivideExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Div);

            public override TNumber Visit(DSLModuloExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Mod);

            public override TNumber Visit(DSLExponentialExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Pow);









            public override TNumber Visit(DSLUnaryLogicalNotExpression expr, VisitContext ctx)
                => ctx.Op.NegLogical(v(expr.Child, ctx));


            public override TNumber Visit(DSLCompareGreaterOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Ge);

            public override TNumber Visit(DSLCompareGreaterThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Gt);

            public override TNumber Visit(DSLCompareLessOrEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Le);

            public override TNumber Visit(DSLCompareLessThanExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Lt);

            public override TNumber Visit(DSLCompareIsEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Eq);

            public override TNumber Visit(DSLCompareIsNotEqualExpression expr, VisitContext ctx)
                => visitBinary(expr, ctx, ctx.Op.Ne);





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

                return (TNumber) ctx.CoreContext.Functions[signature].DynamicInvoke(args.Select(e => (object)e).ToArray());
            }
        }
    }
}
