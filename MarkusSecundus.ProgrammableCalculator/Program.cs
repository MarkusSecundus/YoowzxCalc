﻿using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics.Impl;
using MarkusSecundus.ProgrammableCalculator.Parser;
using MarkusSecundus.Util;
using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{
    public class Program
    {

        public static IExpressionEvaluator<DoubleNumber> MakeExpressionEvaluator() => new ASTInvocationContext<DoubleNumber>(IASTBuilder.Instance, DoubleNumber.ConstantParser.Instance);
        static ASTInvocationContext<DoubleNumber> makeExpressionEvaluator() => new ASTInvocationContext<DoubleNumber>(IASTBuilder.Instance, DoubleNumber.ConstantParser.Instance);


        public static void Main() => test12();

        public static void test12()
        {
            var tree = IASTBuilder.Instance.Build(@"f(x) := f(y) + c");
            Console.WriteLine(tree.Accept(ASTRecursionDetector.Instance, default));
        }

        public static void test11()
        {
            var parseCtx = makeExpressionEvaluator();

            DoubleNumber num1 = 10;
            Func<DoubleNumber, DoubleNumber> aa = null;
            Expression<Func<DoubleNumber, DoubleNumber>> a = x => x.Lt(10).IsZero() ? x.Neg() : aa(x-1);
            var b = parseCtx.ParseToTree(@"b(x) := x < 10 ? b(x-1) : -x");

            WriteLine(a);
            WriteLine();
            WriteLine(b.ExpressionTree);
        }

        public static void test10()
        {
            var parseCtx = makeExpressionEvaluator();

            Expression<Func<DoubleNumber, DoubleNumber>> a = x => x * x + 10;
            var b = parseCtx.Parse(@"b(x) :=10");

            Console.WriteLine(a.Compile()(1).ToString()); 
            Console.WriteLine(b.Wrap<DoubleNumber>()(1));
        }

        public static void test9()
        {
            IExpressionEvaluator<DoubleNumber> parseCtx = new ASTInvocationContext<DoubleNumber>(IASTBuilder.Instance, DoubleNumber.ConstantParser.Instance);

            Func<int> b = () => 1243;
            Delegate c = parseCtx.Parse(@"456+78");

            Delegate a = c;

            Console.WriteLine(a.ArgumentsCount() + ": "+ a.Method.GetParameters().Concat());

        }
        public static void test8()
        {
            IExpressionEvaluator<DoubleNumber> parseCtx = new ASTInvocationContext<DoubleNumber>(IASTBuilder.Instance, DoubleNumber.ConstantParser.Instance);

            parseCtx = parseCtx.WithFunctions(
                @"fact(x) := x<=1 ? 1 : x*fact(x-1)",
                @"fib(x) := x<=1 ? 1 : fib(x-1) + fib(x-2)",
                @"circ(r) := 2*PI*r",
                @"PI() := 3.1415"
                );

            var result = parseCtx.Context["circ"];

            Delegate del = parseCtx.Parse(@"g(x, y) := x > 10 ? x + y : g(x+1, y)");

            //Console.WriteLine((result as Func<DoubleNumber, DoubleNumber>)(5).ToString());
            WriteLine(del.Wrap<DoubleNumber>()(13, 100).ToString());

            
        }

        public static void test7()
        {
            var outerParams = new ParameterExpression[] { Expression.Parameter(typeof(int), "a") };
            var innerParams = outerParams;//new ParameterExpression[] { Expression.Parameter(typeof(int), "a") };

            var recursion = Expression.Parameter(typeof(Func<int, int>), "self");

            var e = Expression.Lambda<Func<int, int>>(
                Expression.Block(
                       recursion.Enumerate(),
                       Expression.Invoke(
                           Expression.Assign(recursion, 
                            Expression.Lambda(
                                Expression.Condition(
                                        Expression.LessThan(innerParams[0], Expression.Constant(1)),
                                            Expression.Constant(1),
                                            Expression.Multiply(innerParams[0], Expression.Invoke(recursion, Expression.Subtract(innerParams[0], Expression.Constant(1))))
                                )
                            
                            , innerParams)
                       ),
                           outerParams)
                ), outerParams
            );
            Console.WriteLine(e.Compile()(5));
        }

        public static void test6()
        {
            var fncs = new Func<int[], int>[10];
            var f = Expression.Lambda(Expression.Invoke(Expression.ArrayIndex(Expression.Constant(fncs),Expression.Constant(3)), Expression.Constant(13)));

            fncs[3] = x => x[0] * x[0];
            Console.WriteLine(f.Compile().DynamicInvoke());
        }

        public static void test5()
        {
            Func<int, int> f = x => 2310 + x;
            var e = Expression.Lambda(Expression.Invoke(Expression.Constant(f), Expression.Constant(1)));
            Console.WriteLine(e.Compile().DynamicInvoke());
        }


        public delegate T f<T>(T[] args);

        public static void test4()
        {
            var arr = new int[] { 11, 12, 13, 14 };
            var param = Expression.Parameter(typeof(ReadOnlySpan<int>), "args");
            var e = Expression.Lambda<f<int>>(Expression.ArrayAccess(param, Expression.Constant(3)), param);
            Console.WriteLine(e.Compile()(arr));
        }


        public static void test3()
        {
            var param1 = Expression.Parameter(typeof(int), "\n\x07");
            var param2 = Expression.Parameter(typeof(int), "\n\x07");
            var expr = Expression.Lambda<Func<int, int, int>>(Expression.Add(param1, param2), param1, param2);
        }

        public static void test2()
        {
            IExpressionEvaluator<DoubleNumber> parseCtx = new ASTInvocationContext<DoubleNumber>(IASTBuilder.Instance, DoubleNumber.ConstantParser.Instance);

            parseCtx = parseCtx.WithFunctions(@"f1(x) := x + 10");
            parseCtx = parseCtx.WithFunctions(@"f2(x) := f1(x+1)");

            var result = parseCtx.Context["f2"];

            Console.WriteLine((result as Func<DoubleNumber, DoubleNumber>)( 3).ToString());
        }

        public static void tst()
        {
            Expression<Func<int, int, int>> eee = (x, y) => x + y;

            Expression q = ((BinaryExpression)eee.Body).Left;
            Console.WriteLine(q);
            Console.WriteLine(Expression.Parameter(typeof(int), "q"));


            DoubleNumber a = 32.32, b = 123.1;
            Func<DoubleNumber, DoubleNumber > add = default(DoubleNumber).Sub;
            var variable = Expression.RuntimeVariables(Expression.Parameter(typeof(DoubleNumber), "dsa"));
            
            var addition = Expression.Call(variable, add.Method, Expression.Constant(b));


            var e = Expression.Lambda<Func<DoubleNumber>>(addition);

            Console.WriteLine(e.Compile()().ToString());

            //Console.WriteLine(IASTBuilder.Instance.Build(@"f(x):= 12.3e0 & 10"));  
        }

    }
}
