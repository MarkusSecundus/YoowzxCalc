using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public static class YCCompilerUtils
    {
        internal static Type GetFuncType<T>(this YCFunctionSignature<T> self)
            => Expression.GetFuncType(typeof(T).Repeat(self.ArgumentsCount + 1).ToArray());
        internal static Type GetExpressionFuncType<T>(this YCFunctionSignature<T> self)
            => typeof(Expression<>).MakeGenericType(self.GetFuncType());


        public static YCFunctionSignature<TNumber> GetDelegateTypeSignature<TNumber, TDelegate>(string name) where TDelegate: Delegate
        {
            var header = FunctionUtil.GetDelegateTypeParameters<TDelegate>();
            return new YCFunctionSignature<TNumber>(name, header.Count);
        } 

        public static YCFunctionSignature<TNumber> GetSignature<TNumber>(this YCFunctionDefinition self)
            => new() { Name = self.Name, ArgumentsCount = self.Arguments.Count };
        public static YCFunctionSignature<TNumber> GetSignature<TNumber>(this YCFunctioncallExpression self)
            => new() { Name = self.Name, ArgumentsCount = self.Arguments.Count };


        public static YCFunctionSignature<TNumber> GetSignature<TNumber>(this Delegate self, string name)
            => self.GetSignature<TNumber, Delegate>(name);


        public static YCFunctionSignature<TNumber> GetSignature<TNumber, TDelegate>(this TDelegate self, string name) where TDelegate : Delegate
        {
            //if (FunctionUtil.IsConcreteDelegateType<TDelegate>())
            //    return GetDelegateTypeSignature<TNumber, TDelegate>(name);

            var parameters = self.GetParameters();
            if (!typeof(TNumber).IsAssignableFrom(self.Method.ReturnType) 
                || parameters.Any(p => !p.ParameterType.IsAssignableFrom(typeof(TNumber)))
                ) 
                throw new ArgumentException($"Method must have only arguments of type {typeof(TNumber)}", nameof(self));

            return new() { Name = name, ArgumentsCount = parameters.Length };
        }







        internal delegate TNumber ExpressionDelegate<TNumber>(params TNumber[] args);

        internal static Expression<ExpressionDelegate<TNumber>> WrapParams<TNumber>(this Delegate self)
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

        internal static Expression<TDelegate> UnwrapArrayParams<TNumber, TDelegate>(this ExpressionDelegate<TNumber> self, int argsCount)
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
