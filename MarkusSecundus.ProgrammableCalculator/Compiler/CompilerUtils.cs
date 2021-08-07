using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{
    static class CompilerUtils
    {
        public static Type GetFuncType<T>(this FunctionSignature<T> self)
            => Expression.GetFuncType(typeof(T).Repeat(self.ArgumentsCount + 1).ToArray());
        public static Type GetExpressionFuncType<T>(this FunctionSignature<T> self)
            => typeof(Expression<>).MakeGenericType(self.GetFuncType());



        public static FunctionSignature<TNumber> GetSignature<TNumber>(this DSLFunctionDefinition self)
            => new() { Name = self.Name, ArgumentsCount = self.Arguments.Count };
        public static FunctionSignature<TNumber> GetSignature<TNumber>(this DSLFunctioncallExpression self)
            => new() { Name = self.Name, ArgumentsCount = self.Arguments.Count};


        public delegate TNumber ExpressionDelegate<TNumber>(params TNumber[] args);

        public static Expression<ExpressionDelegate<TNumber>> WrapParams<TNumber>(this Delegate self)
        {
            var args = Expression.Parameter(typeof(TNumber[]), "#args");
            var argsPassed = new Expression[self.ArgumentsCount()];

            for (int t = 0; t < argsPassed.Length; ++t)
                argsPassed[t] = Expression.ArrayAccess(args, Expression.Constant(t));

            return Expression.Lambda<ExpressionDelegate<TNumber>>(
                Expression.Invoke(Expression.Constant(self), argsPassed),
                args
            );
        }

        public static Expression<TDelegate> UnwrapArrayParams<TNumber, TDelegate>(this ExpressionDelegate<TNumber> self, int argsCount)
        {
            var argParams = new ParameterExpression[argsCount];
            for (int t = argsCount; --t >= 0;) argParams[t] = Expression.Parameter(typeof(TNumber));

            return Expression.Lambda<TDelegate>(
                Expression.Invoke(Expression.Constant(self), Expression.NewArrayInit(typeof(TNumber), argParams)),
                argParams
            );
        }









    }
}
