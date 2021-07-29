using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    static class ParserUtils
    {
        public static Type GetFuncType<T>(this DSLFunctionDefinition self) 
            => Expression.GetFuncType(typeof(T).Repeat(self.Arguments.Count + 1).ToArray());

        public static Type GetExpressionFuncType<T>(this DSLFunctionDefinition self)
            => typeof(Expression<>).MakeGenericType(self.GetFuncType<T>());



        public static ExpressionDelegate<TNumber> Wrap<TNumber>(this Delegate self) where TNumber: INumber<TNumber>
        {
            var args = Expression.Parameter(typeof(TNumber[]), "#args");
            var argsPassed = new Expression[self.ArgumentsCount()];

            for (int t = 0; t < argsPassed.Length; ++t)
                argsPassed[t] = Expression.ArrayAccess(args, Expression.Constant(t));

            return Expression.Lambda<ExpressionDelegate<TNumber>>(
                Expression.Invoke(Expression.Constant(self), argsPassed),
                args
            ).Compile();
        }


        private static readonly Type _ClosureType = Expression.Lambda(Expression.Constant(0)).Compile().Method.GetParameters()[0].GetType();

        public static int ArgumentsCount(this Delegate self)
        {
            var parameters = self.Method.GetParameters();
            return parameters.Length - (parameters.Length > 0 && _ClosureType.IsInstanceOfType(parameters[0]) ? 1 : 0);
        }
    }
}
