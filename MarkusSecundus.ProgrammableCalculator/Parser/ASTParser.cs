using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser 
{ 


    class ASTContext<TNumber> : IExpressionEvaluator<TNumber> where TNumber : INumber<TNumber>
    {
        internal ImmutableDictionary<string, LambdaExpression> Functions { get; }

        public IReadOnlyDictionary<string, IExpression<TNumber>> Context => throw new NotImplementedException();

        public IExpression<TNumber> Parse(string expression)
        {
            throw new NotImplementedException();
        }

        public IExpressionEvaluator<TNumber> WithFunctions(IEnumerable<string> expressions)
        {
            throw new NotImplementedException();
        }
    }


    class ASTParseContext
    {
        public ImmutableDictionary<string, ParameterExpression> Parameters { get; init; }
        

    }

    class ASTParser<TNumber> : DSLVisitorBase<Expression, ASTParseContext> where TNumber : INumber<TNumber>, new()
    {
        private readonly IConstantParser<TNumber> _constantParser;

        public ASTParser(IConstantParser<TNumber> constantParser) => _constantParser = constantParser;

        private Expression v(DSLExpression e, ASTParseContext ctx) => e.Accept(this, ctx);


        private static TNumber T = new();

        private static readonly Func<TNumber> _UnaryMinus = T.Neg, _LogicalNegation = T.NegLogical;
        private static readonly Func<bool> _IsZero = T.IsZero;

        private static readonly Func<TNumber, TNumber> _Add = T.Add, _Sub = T.Sub, _Mul = T.Mul, _Div = T.Div, _Mod = T.Mod, _Pow = T.Pow;

        private static readonly Func<TNumber, TNumber> _Le = T.Le, _Lt = T.Lt, _Ge = T.Ge, _Gt = T.Gt, _Eq = T.Eq, _Ne = T.Ne;


        public override Expression Visit(DSLConstantExpression expr, ASTParseContext ctx)
        {
            return Expression.Constant(_constantParser.Parse(expr.Value));
        }

        public override Expression Visit(DSLFunctionDefinition expr, ASTParseContext ctx)
        {
            var parameters = expr.Arguments.Select(p => Expression.Parameter(typeof(TNumber), p)).ToArray();
            var parametersDict = parameters.Select(p => new KeyValuePair<string, ParameterExpression>(p.Name, p)).ToImmutableDictionary();

            ctx = new ASTParseContext { Parameters = parametersDict };

            var body = v(expr.Body, ctx);
            return Expression.Lambda(body, parameters);
        }

        public override Expression Visit(DSLArgumentExpression expr, ASTParseContext ctx)
        {
            return ctx.Parameters[expr.ArgumentName];
        }

        public override Expression Visit(DSLUnaryMinusExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.Child, ctx), _UnaryMinus.Method);
        }

        public override Expression Visit(DSLUnaryPlusExpression expr, ASTParseContext ctx)
        {
            return v(expr.Child, ctx);
        }

        public override Expression Visit(DSLAddExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Add.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLSubtractExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Sub.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLMultiplyExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Mul.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLDivideExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Div.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLModuloExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Mod.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLExponentialExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Pow.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLFunctioncallExpression expr, ASTParseContext ctx)
        {
            return null;
        }

        public override Expression Visit(DSLUnaryLogicalNotExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.Child, ctx), _LogicalNegation.Method);
        }

        public override Expression Visit(DSLCompareGreaterOrEqualExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Ge.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareGreaterThanExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Gt.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareLessOrEqualExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Le.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareLessThanExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Lt.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareIsEqualExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Eq.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareIsNotEqualExpression expr, ASTParseContext ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Ne.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLLogicalAndExpression expr, ASTParseContext ctx)
        {
            var left = v(expr.LeftChild, ctx);
            var right = v(expr.RightChild, ctx);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, left, right);
        }

        public override Expression Visit(DSLLogicalOrExpression expr, ASTParseContext ctx)
        {
            var left = v(expr.LeftChild, ctx);
            var right = v(expr.RightChild, ctx);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, right, left);
        }

        public override Expression Visit(DSLConditionalExpression expr, ASTParseContext ctx)
        {
            var condition = Expression.Call(v(expr.Condition, ctx), _IsZero.Method);
            return Expression.Condition(condition, v(expr.IfFalse, ctx), v(expr.IfTrue, ctx));
        }
    }
}
