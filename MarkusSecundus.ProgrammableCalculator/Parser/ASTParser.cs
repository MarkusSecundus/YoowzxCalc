using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser 
{ 


    class ASTInvocationContext<TNumber> : IExpressionEvaluator<TNumber> where TNumber : INumber<TNumber>
    {
        private readonly ASTParser<TNumber> _parser;
        private readonly IASTBuilder _builder;

        public ASTInvocationContext(IASTBuilder astBuilder, IConstantParser<TNumber> constantParser)
        {
            _builder = astBuilder;
            _parser = new ASTParser<TNumber>(constantParser);
            FunctionsRaw = ImmutableDictionary<string, MutableWrapper<LambdaExpression>>.Empty;
            FunctionsFuncTypes = ImmutableDictionary<string, Type>.Empty;
            Context = makeContext();
        }

        private ASTInvocationContext(ASTInvocationContext<TNumber> father, IEnumerable<string> expressions)
        {
            _builder = father._builder;
            _parser = father._parser;

            var toAdd = expressions.Select(_builder.Build).ToArray();

            FunctionsRaw = father.FunctionsRaw.Chain(toAdd.Select(f => (f.Name, new MutableWrapper<LambdaExpression>()).AsKV())).ToImmutableDictionary();
            FunctionsFuncTypes = father.FunctionsFuncTypes.Chain(toAdd.Select(f => (f.Name, f.GetExpressionFuncType<TNumber>()).AsKV())).ToImmutableDictionary();

            foreach(var f in toAdd)
            {
                FunctionsRaw[f.Name].Value = (LambdaExpression)f.Accept(_parser, new ASTParseContext<TNumber>(this));
            }
            Context = makeContext();
        }

        private DefaultValDict<string, Delegate> makeContext() => new(key => FunctionsRaw[key].Value.Compile());

        internal ImmutableDictionary<string, MutableWrapper<LambdaExpression>> FunctionsRaw { get; }
        internal ImmutableDictionary<string, Type> FunctionsFuncTypes { get; }

        public IReadOnlyDictionary<string, Delegate> Context { get; }





        public Delegate Parse(string expression)
        {
            var ast = _builder.Build(expression);
            var expr = (LambdaExpression)ast.Accept(_parser, new ASTParseContext<TNumber>(this));
            return expr.Compile();
        }

        public IExpressionEvaluator<TNumber> WithFunctions(IEnumerable<string> expressions)
            => new ASTInvocationContext<TNumber>(this, expressions);
    }




    class ASTParseContext<TNumber>  where TNumber : INumber<TNumber>
    {
        public ASTParseContext(ASTInvocationContext<TNumber> ctx)
            => InvocationContext = ctx;

        public ImmutableDictionary<string, ParameterExpression> Parameters { get; init; }

        public string ThisFunctionName { get; init; }
        public ParameterExpression ThisFunction { get; init; }
        
        public ASTInvocationContext<TNumber> InvocationContext { get; init; }
    }





    class ASTParser<TNumber> : DSLVisitorBase<Expression, ASTParseContext<TNumber>> where TNumber : INumber<TNumber>
    {
        private readonly IConstantParser<TNumber> _constantParser;

        public ASTParser(IConstantParser<TNumber> constantParser) => _constantParser = constantParser;

        private Expression v(DSLExpression e, ASTParseContext<TNumber> ctx) => e.Accept(this, ctx);


        private static readonly TNumber T = (TNumber) FormatterServices.GetUninitializedObject(typeof(TNumber));



        private static readonly Func<TNumber> _UnaryMinus = T.Neg, _LogicalNegation = T.NegLogical;
        private static readonly Func<bool> _IsZero = T.IsZero;

        private static readonly Func<TNumber, TNumber> _Add = T.Add, _Sub = T.Sub, _Mul = T.Mul, _Div = T.Div, _Mod = T.Mod, _Pow = T.Pow;

        private static readonly Func<TNumber, TNumber> _Le = T.Le, _Lt = T.Lt, _Ge = T.Ge, _Gt = T.Gt, _Eq = T.Eq, _Ne = T.Ne;


        public override Expression Visit(DSLConstantExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Constant(_constantParser.Parse(expr.Value));
        }

        public override Expression Visit(DSLFunctionDefinition expr, ASTParseContext<TNumber> ctx)
        {
            var parameters = expr.Arguments.Select(p => Expression.Parameter(typeof(TNumber), p)).ToArray();
            var parametersDict = parameters.Select(p => new KeyValuePair<string, ParameterExpression>(p.Name, p)).ToImmutableDictionary();

            ctx = new(ctx.InvocationContext)
            {
                Parameters = parametersDict,
                ThisFunctionName = expr.Name,
                ThisFunction = Expression.Parameter(expr.GetFuncType<TNumber>(), expr.Name)
            };

            var body = v(expr.Body, ctx);

            return Expression.Lambda(
                Expression.Block(
                    ctx.ThisFunction.Enumerate(),
                    Expression.Invoke(
                        Expression.Assign(
                            ctx.ThisFunction, 
                            Expression.Lambda(body, parameters)
                        ),
                        parameters)
                ), 
                parameters);
        }

        public override Expression Visit(DSLArgumentExpression expr, ASTParseContext<TNumber> ctx)
        {
            if (ctx.Parameters.TryGetValue(expr.ArgumentName, out var ret))
                return ret;
            return v(new DSLFunctioncallExpression { Name = expr.ArgumentName, Arguments = CollectionsUtils.EmptyList<DSLExpression>() }, ctx);
        }

        public override Expression Visit(DSLUnaryMinusExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.Child, ctx), _UnaryMinus.Method);
        }

        public override Expression Visit(DSLUnaryPlusExpression expr, ASTParseContext<TNumber> ctx)
        {
            return v(expr.Child, ctx);
        }

        public override Expression Visit(DSLAddExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Add.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLSubtractExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Sub.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLMultiplyExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Mul.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLDivideExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Div.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLModuloExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Mod.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLExponentialExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Pow.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLFunctioncallExpression expr, ASTParseContext<TNumber> ctx)
        {
            Expression functionPointer;
            if (expr.Name == ctx.ThisFunctionName)
            {
                functionPointer = ctx.ThisFunction;
            }
            else if (!ctx.InvocationContext.FunctionsRaw.TryGetValue(expr.Name, out var functionWrapper))
            {
                throw new ParserException($"No function with name {expr.Name} was found");
            }
            else if(functionWrapper.Value != null)
            {
                functionPointer = Expression.Constant(functionWrapper.Value);
            }
            else
            {
                var functionPointerRaw = Expression.PropertyOrField(Expression.Constant(functionWrapper), nameof(functionWrapper.Value));
                functionPointer = Expression.Convert(functionPointerRaw, ctx.InvocationContext.FunctionsFuncTypes[expr.Name]);
            }

            var args = expr.Arguments.Select(a => v(a, ctx));

            return Expression.Invoke(functionPointer, args);
        }

        public override Expression Visit(DSLUnaryLogicalNotExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.Child, ctx), _LogicalNegation.Method);
        }

        public override Expression Visit(DSLCompareGreaterOrEqualExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Ge.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareGreaterThanExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Gt.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareLessOrEqualExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Le.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareLessThanExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Lt.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareIsEqualExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Eq.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLCompareIsNotEqualExpression expr, ASTParseContext<TNumber> ctx)
        {
            return Expression.Call(v(expr.LeftChild, ctx), _Ne.Method, v(expr.RightChild, ctx));
        }

        public override Expression Visit(DSLLogicalAndExpression expr, ASTParseContext<TNumber> ctx)
        {
            var left = v(expr.LeftChild, ctx);
            var right = v(expr.RightChild, ctx);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, left, right);
        }

        public override Expression Visit(DSLLogicalOrExpression expr, ASTParseContext<TNumber> ctx)
        {
            var left = v(expr.LeftChild, ctx);
            var right = v(expr.RightChild, ctx);

            var condition = Expression.Call(left, _IsZero.Method);
            return Expression.Condition(condition, right, left);
        }

        public override Expression Visit(DSLConditionalExpression expr, ASTParseContext<TNumber> ctx)
        {
            var condition = Expression.Call(v(expr.Condition, ctx), _IsZero.Method);
            return Expression.Condition(condition, v(expr.IfFalse, ctx), v(expr.IfTrue, ctx));
        }
    }
}
