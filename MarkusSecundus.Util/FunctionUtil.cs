﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public static class FunctionUtil
    {

        private static readonly Type _ClosureType = Expression.Lambda(Expression.Constant(0)).Compile().Method.GetParameters()[0].GetType();

        public static int ArgumentsCount(this Delegate self) => self.GetParameters().Length;

        public static ParameterInfo[] GetParameters(this Delegate self)
        {
            var parameters = self.Method.GetParameters();
            return (parameters.Length > 0 && parameters[0].ParameterType.FullName == "System.Runtime.CompilerServices.Closure")
                ? parameters[1..]
                : parameters;
        }











        internal struct FunctionParameters
        {
            public Type[] Args { get; init; }
            public Type Ret { get; init; }
        }
        internal static FunctionParameters GetFunctionParameters(this Delegate self)
        {
            if (self.Method.ReturnType == typeof(void))
                throw new ArgumentException($"Delegate must have a return value", nameof(self));
            return new() { Ret = self.Method.ReturnType, Args = self.GetParameters().Select(p => p.ParameterType).ToArray() };
        }




        public static Delegate Entuplize(this Delegate self)
        {
            var parameters = self.GetFunctionParameters();

            var tupleType = TupleUtils.GetValueTupleType(parameters.Args);

            var tupleParam = Expression.Parameter(tupleType, "#args_tuple");

            return Expression.Lambda
            (
                Expression.Invoke
                (
                        Expression.Constant(self),
                        getTupleAccesses(tupleParam, parameters.Args)
                ),
                tupleParam
            ).Compile();
        }

        private static Expression[] getTupleAccesses(Expression tuple, Type[] elementTypes)
        {
            var ret = new Expression[elementTypes.Length];
            for(int t = Math.Min(elementTypes.Length, TupleUtils.MaxNormalTupleSize); --t >= 0;)
            {
                ret[t] = Expression.PropertyOrField(tuple, "Item"+(t+1));
            }
            if(elementTypes.Length > TupleUtils.MaxNormalTupleSize)
            {
                var rest = getTupleAccesses(Expression.PropertyOrField(tuple, "Rest"), elementTypes[TupleUtils.MaxNormalTupleSize..]);
                Array.Copy(rest, 0, ret, TupleUtils.MaxNormalTupleSize, rest.Length);
            }
            return ret;
        }


        public static Delegate Detuplize(this Delegate self)
        {
            var parameters = self.GetFunctionParameters();
            if (!(parameters.Args.Length == 1 && parameters.Args[0].IsValueTupleType()))
                throw new ArgumentException($"The function must have only one argument that is ValueTuple!", nameof(self));

            var argType = parameters.Args[0];

            var args = argType.GetValueTupleElementTypes().Select(type => Expression.Parameter(type)).ToArray();

            return Expression.Lambda
            (
                Expression.Invoke(Expression.Constant(self), getTupleCreation(argType, args)),
                args
            ).Compile();
        }

        public static Expression getTupleCreation(Type tupleType, Expression[] elements)
        {
            if (elements.Length >= TupleUtils.MaxNormalTupleSize)
            {
                var rest = elements[TupleUtils.MaxNormalTupleSize..];
                var restType = TupleUtils.GetValueTupleType(rest.Select(e=>e.Type).ToArray());
                elements = elements[..TupleUtils.MaxNormalTupleSize].Concat(getTupleCreation(restType, rest));
            }
            return Expression.New(
                tupleType.GetConstructor(elements.Select(e=>e.Type).ToArray()),
                elements
                );
        }

        public static TDelegate Autocached<TDelegate>(this TDelegate function) where TDelegate: Delegate
            => Caching.Autocached(function) as TDelegate;

        internal static class Caching
        {
            internal static Delegate Autocached(Delegate function)
            {
                var parameters = function.GetFunctionParameters();

                if (parameters.Args.Length <= 0)
                {
                    return doArgumentlessAutocaching(function);
                }

                var tupleType = TupleUtils.GetValueTupleType(parameters.Args);
                var cache = createCache(function);

                var tupleParam = Expression.Parameter(tupleType, "#args_tuple");

                var ret = Expression.Lambda
                (
                    Expression.Property(Expression.Constant(cache), "Item", tupleParam),
                    tupleParam
                );
                return ret.Compile().Detuplize();
            }



            private static object createCache(Delegate function)
            {
                var parameters = function.GetFunctionParameters();
                if (parameters.Args.Length >= TupleUtils.TupleTypesByArgsCount.Length)
                    throw new NotImplementedException($"Autocaching not supported for functions with more than {TupleUtils.TupleTypesByArgsCount.Length - 1} arguments");
                var returnType = function.Method.ReturnType;

                var tupleType = TupleUtils.GetValueTupleType(parameters.Args);

                var cacheTypeParameters = new Type[] { tupleType, returnType };
                var cacheType = typeof(DefaultValDict<,>).MakeGenericType(cacheTypeParameters);

                return Activator.CreateInstance(cacheType, function.Entuplize(), null);
            }




            private static Delegate doArgumentlessAutocaching(Delegate function)
            {
                var parameters = function.GetFunctionParameters();
                if (!parameters.Args.IsEmpty())
                    throw new ArgumentException($"Delegate must be a method with no parameters", nameof(function));

                SettableOnce<object> dummyWrap = default;
                object wrap = Activator.CreateInstance(typeof(SettableOnce<>).MakeGenericType(function.Method.ReturnType));
                var wrapParam = Expression.Constant(wrap);
                var wrapParamValue = Expression.PropertyOrField(wrapParam, nameof(dummyWrap.Value));

                return Expression.Lambda
                (
                    Expression.Condition(
                            test: Expression.PropertyOrField(wrapParam, nameof(dummyWrap.IsSet)),
                            ifTrue: wrapParamValue,
                            ifFalse: Expression.Assign(wrapParamValue, Expression.Invoke(Expression.Constant(function)))
                    )
                ).Compile();
            }

        }
    }
}