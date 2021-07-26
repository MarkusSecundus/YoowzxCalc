using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    class ASTParser<TNumber> : DSLVisitorBaseNoContext<Expression, object> where TNumber : INumber<TNumber>, new()
    {
        private readonly IConstantParser<TNumber> _constantParser;

        public ASTParser(IConstantParser<TNumber> constantParser) => _constantParser = constantParser;

        private Expression v(DSLExpression e) => e.Accept(this, null);


        private static TNumber T = new();

        private static readonly Func<TNumber> _UnaryMinus = T.Neg, _LogicalNegation = T.NegLogical;
        private static readonly Func<bool> _IsZero = T.IsZero;

        private static readonly Func<TNumber, TNumber> _Add = T.Add, _Sub = T.Sub, _Mul = T.Mul, _Div = T.Div, _Mod = T.Mod, _Pow = T.Pow;

        private static readonly Func<TNumber, TNumber> _Le = T.Le, _Lt = T.Lt, _Ge = T.Ge, _Gt = T.Gt, _Eq = T.Eq, _Ne = T.Ne;


        public override Expression Visit(DSLConstantExpression expr)
        {
            return Expression.Constant(_constantParser.Parse(expr.Value));
        }

        public override Expression Visit(DSLFunctionDefinition expr)
        {
            var body = v(expr.Body);
            for(int t = 0; t < expr.Arguments.Count; ++t)
            {
                //body = Expression.RuntimeVariables()
            }

            return Expression.Lambda<ExpressionDelegate<TNumber>>(body,true, Expression.Parameter(typeof(Span<TNumber>)));
        }

        public override Expression Visit(DSLArgumentExpression expr)
        {
            return Expression.Parameter(typeof(TNumber), expr.ArgumentName);
        }

        public override Expression Visit(DSLUnaryMinusExpression expr)
        {
            return Expression.Call(v(expr.Child), _UnaryMinus.Method);
        }

        public override Expression Visit(DSLUnaryPlusExpression expr)
        {
            return v(expr.Child);
        }

        public override Expression Visit(DSLAddExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Add.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLSubtractExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Sub.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLMultiplyExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Mul.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLDivideExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Div.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLModuloExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Mod.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLExponentialExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Pow.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLFunctioncallExpression expr)
        {
            return null;
        }

        public override Expression Visit(DSLUnaryLogicalNotExpression expr)
        {
            return Expression.Call(v(expr.Child), _LogicalNegation.Method);
        }

        public override Expression Visit(DSLCompareGreaterOrEqualExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Ge.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLCompareGreaterThanExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Gt.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLCompareLessOrEqualExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Le.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLCompareLessThanExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Lt.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLCompareIsEqualExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Eq.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLCompareIsNotEqualExpression expr)
        {
            return Expression.Call(v(expr.LeftChild), _Ne.Method, v(expr.RightChild));
        }

        public override Expression Visit(DSLLogicalAndExpression expr)
        {
            var left = v(expr.LeftChild);
            var right = v(expr.RightChild);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, left, right);
        }

        public override Expression Visit(DSLLogicalOrExpression expr)
        {
            var left = v(expr.LeftChild);
            var right = v(expr.RightChild);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, right, left);
        }

        public override Expression Visit(DSLConditionalExpression expr)
        {
            var condition = Expression.Call(v(expr.Condition), _IsZero.Method);
            return Expression.Condition(condition, v(expr.IfFalse), v(expr.IfTrue));
        }
    }
}
