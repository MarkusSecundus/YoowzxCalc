using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.Compiler;
using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.Compiler.Impl;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{

    public static class Program
    {
        static readonly Func<double, double> Sin = Math.Sin, Cos = Math.Cos, F=x=>100*x;


        public static void Main() => test9();


        class A
        {
            public int T { get; set; }
            public int this[int i] => i;
            public int this[int i, int j] => i + j;
        }
        
        public static void test10()
        {
            foreach (var v in typeof(A).GetProperties())
                WriteLine(v);
        }

        public static void test9()
        {
            Func<long, long> fib = null;
            fib = x => x <= 1 ? x : fib(x - 1) + fib(x - 2);
            fib = fib.Autocached();
            for (int t = 0; t < 80; ++t)
                WriteLine(fib(t));
        }
        public static void test8()
        {
            Func<int, int, int, int, int, int, int, int, int, int, string, string> f = (x, y, z, a, b, c, d, e, f, g, h) => x * 2 + y + z + h;
            var ff = (Func<(int, int, int, int, int, int, int, int, int, int, string), string>) FunctionUtil.Entuplize(f);
            WriteLine(ff((1, 10, 100, 1, 1, 1, 1, 1, 1, 1, "a")));
            WriteLine(ff.GetParameters().MakeString());
            var fff = (Func<int, int, int, int, int, int, int, int, int, int, string, string>)FunctionUtil.Detuplize(ff);
            WriteLine(fff(1, 10, 100, 1, 1, 1, 1, 1, 1, 1, "v"));
        }

        public static void test7()
        {
            Console.WriteLine((1, 2, "", 2, "", 2, "", 2, "", 2, "", 2, "", 2, "", 2, "", 2, "").GetType().GetValueTupleElementTypes().MakeString());
        }

        public static void test6()
        {
            Func<int, int> f = x => x*2;
            var ff = FunctionUtil.Entuplize(f) as Func<ValueTuple<int>, int>;
            WriteLine(ff(ValueTuple.Create(1)));
        }

        public static void test5()
        {
            Console.WriteLine(new int[] { 1, 2, 3, 4, 5 }[..^1].MakeString());
            Console.WriteLine(new int[] { 1, 2, 3, 4, 5 }[1..].MakeString());
            Console.WriteLine(new int[] { 1, 2, 3, 4, 5 }[^1]);
        }

        public static void test4()
        {
            WriteLine(default((int, string, int, int, int, int, int, object, double)).GetType());
            WriteLine(TupleUtils.GetValueTupleType(typeof(int), typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(object), typeof(double)));
        }


        public static void test3()
        {
            foreach(var (type, i) in TupleUtils.TupleTypesByArgsCount.Zip(CollectionsUtils.InfiniteRange()))
            {
                Console.WriteLine($"{i}: {type}");
            }
        }



        public static void test2()
        {
            IASTBuilder builder = IASTBuilder.Instance;
            IASTInterpreter<double> interpreter = new ASTInterpreter<double>(new BasicNumberOperators.Double());
            IASTCompiler<double> compiler = new ASTCompiler<double>(new BasicNumberOperators.Double());
            var ctx = IASTFunctioncallContext.Make<double>().ResolveSymbols
            (
                (new FunctionSignature<double>("sin", 1), Sin),
                (new FunctionSignature<double>("cos", 1), Cos),
                (new FunctionSignature<double>("f", 1), F)
            );


            var tree = builder.Build("f(x) := sin(x)**-2 + cos(x)**-2");


            for (double x = 0; x < 7; x += 0.1)
            {
                double a, b;
                Write("{0} ", a = interpreter.Interpret(ctx, tree, x));
                Write(b = (double)compiler.Compile(ctx, tree).Compile().DynamicInvoke(x));
                WriteLine(a == b ? "" : " !");
            }
        }


        public static void test1()
        {
            IASTBuilder builder = IASTBuilder.Instance;
            IASTInterpreter<double> interpreter = new ASTInterpreter<double>(new BasicNumberOperators.Double());
            IASTCompiler<double> compiler = new ASTCompiler<double>(new BasicNumberOperators.Double());
            var ctx = IASTFunctioncallContext.Make<double>();


            var tree = builder.Build("f(x) := x<= 1 ? x : f(x-1) + f(x-2)");


            for(double x = 0; x < 32; x+=1)
            {
                double a, b;
                Write("{0} ", a=interpreter.Interpret(ctx, tree, x));
                Write(b = (double)compiler.Compile(ctx, tree).Compile().DynamicInvoke(x));
                WriteLine(a==b?"":" !");
            }
        }

    }
}
