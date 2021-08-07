using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTCompilerWithCaching<TNumber> : IASTCompiler<TNumber>
    {
        private readonly IASTCompiler<TNumber> _base;

        public ASTCompilerWithCaching(IASTCompiler<TNumber> baseCompiler)
            => _base = baseCompiler;

        public IASTCompilationResult<TNumber> Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            if (product is not ASTCompilationResult<TNumber> ret)
                return product;

            if (toCompile.Arguments.Count <= 0)
            {
                return new ASTCompilationResult<TNumber>(doArgumentlessAutocaching(ret), ret.ThisFunctionWrapper);
            }

            var cache = createCache(toCompile, ret.Expression);

            return ret;
        }



        private object createCache(DSLFunctionDefinition def, LambdaExpression function)
        {
            if (def.Arguments.Count >= TupleUtils.TupleTypesByArgsCount.Length)
                throw new NotImplementedException($"Autocaching not supported for functions with more than {TupleUtils.TupleTypesByArgsCount.Length-1} arguments");

            var typeParameters = typeof(TNumber).Repeat(def.Arguments.Count).ToArray();
            var tupleType = TupleUtils.GetValueTupleType(typeParameters);

            var cacheTypeParameters = new Type[] { tupleType, typeof(TNumber) };
            var cacheType = typeof(DefaultValDict<,>).MakeGenericType(cacheTypeParameters);
            var constructor = cacheType.GetConstructor(new[] { typeof(Func<,>).MakeGenericType(cacheTypeParameters) });

            return null;
        }




        private LambdaExpression doArgumentlessAutocaching(ASTCompilationResult<TNumber> ret)
        {
            SettableOnce<TNumber> wrap = new();
            var wrapParam = Expression.Constant(wrap);
            var wrapParamValue = Expression.PropertyOrField(wrapParam, nameof(wrap.Value));

            return Expression.Lambda
            (
                Expression.Condition(
                        test: Expression.PropertyOrField(wrapParam, nameof(wrap.IsSet)),
                        ifTrue: wrapParamValue,
                        ifFalse: Expression.Assign(wrapParamValue, ret.Expression.Body)
                )
            );
        }
    }
}
