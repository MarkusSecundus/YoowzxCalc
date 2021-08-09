using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using static System.Console;

namespace MarkusSecundus.YoowzxCalc
{

    public static class Program
    {

        static readonly Func<double, double> Sin = Math.Sin, Cos = Math.Cos, F=x=>100*x;


        public static void Main() => test16();


        public static void test16()
        {
            var calc = IYoowzxCalculator<double>.Make();

            Func<double, double> ffff = x=> (int)Math.Cos(x);

            calc.AddFunction<Func<double, double>>("sin", Math.Sin)
                .AddFunction<Func<double, double, double>>("sum", (a, b) => a + b)
                .AddFunction("cos", ffff);

            WriteLine(calc.Compile<Func<double, double>>("f(x) := x<= 1 ? 1 : x*f(x-1)")(5));
            WriteLine(calc.Compile<Func<double>>("sum(10, 103) *2")());
             WriteLine(calc.Compile<Func<double, double>>("f(x) := sin(x)**2 + cos(x)**2")(1));

            
            for (int t = 0; t < 200; ++t)
                WriteLine(calc.Compile<Func<double, double>>("[cache]f(x) := x<=1 ? x : f(x-1)+f(x-2)")(t));
        }


        public static void test15()
        {
            IYCAstBuilder builder = IYCAstBuilder.Instance;
            IYCCompiler<double> compiler = IYCCompiler<double>.Make(new BasicNumberOperators.Double());
            IYCCompiler<double> interpreter = IYCInterpreter<double>.Make(new BasicNumberOperators.Double()).AsCompiler();
            var ctx = IASTFunctioncallContext.Make<double>().ResolveSymbols
            (
                (new YCFunctionSignature<double>("sin", 1), Sin),
                (new YCFunctionSignature<double>("cos", 1), Cos),
                (new YCFunctionSignature<double>("f", 1), F)
            );

            var tree = builder.Build("[a, b, c, d, e, f] f(x) := 1 + x*3");

            for(int t = 0; t < 10; ++t)
            {
                WriteLine("{0} {1}", compiler.Compile(ctx, tree).Compile().DynamicInvoke((double)t), interpreter.Compile(ctx, tree).Compile().DynamicInvoke((double)t));
            }

        }
        public static void test14()
        {
            Func<object[], string> f = a => ""+ a[0] + a[1];
            var ff = (Func<string, int, string>)f.Dearrayize(typeof(string), typeof(int));
            Console.WriteLine(ff("das", 321));
        }


        public static void test13()
        {
            IYCAstBuilder builder = IYCAstBuilder.Instance;
            IYCCompiler<double> compiler = IYCCompiler<double>.Make(new BasicNumberOperators.Double());

            var tree = builder.Build("[a, b, c, d, e, f] f(x) := 1 + 2*3");

            WriteLine(tree.Annotations.MakeString());
        }


        public static void test12()
        {
            Func<BigDecimal, BigDecimal> fib = null;
            fib = x => x <= (BigDecimal)1 ? x : fib(x - 1) + fib(x - 2) + (BigDecimal)13e-15m;
            fib = fib.Autocached();
            for (int t = 0; t < 58; ++t)
                WriteLine("{0}\n",fib(t));
        }
        public static void test11()
        {
            IYCAstBuilder builder = IYCAstBuilder.Instance;
            IYCInterpreter<double> interpreter = IYCInterpreter<double>.Make(new BasicNumberOperators.Double());
            IYCCompiler<double> compiler = IYCCompiler<double>.Make(new BasicNumberOperators.Double());
            compiler = IYCCompiler<double>.MakeCached(compiler);
            var ctx = IASTFunctioncallContext.Make<double>().ResolveSymbols
            (
                (new YCFunctionSignature<double>("sin", 1), Sin),
                (new YCFunctionSignature<double>("cos", 1), Cos),
                (new YCFunctionSignature<double>("f", 1), F)
            );


            var tree = builder.Build("f(x) := x<= 1 ? x : f(x-1) + f(x-2)");


            for (double x = 0; x < 250; x += 1)
            {
                double b;
                //Write("{0} ", a = interpreter.Interpret(ctx, tree, x));
                Write(b = (double)compiler.Compile(ctx, tree).Compile().DynamicInvoke(x));
                WriteLine();
                //WriteLine(a == b ? "" : " !");
            }

        }

        

        
        
        
        

        
        
        
        

        
        
        
        
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
            IYCAstBuilder builder = IYCAstBuilder.Instance;
            IYCInterpreter<double> interpreter = IYCInterpreter<double>.Make(new BasicNumberOperators.Double());
            IYCCompiler<double> compiler = IYCCompiler<double>.Make(new BasicNumberOperators.Double());
            var ctx = IASTFunctioncallContext.Make<double>().ResolveSymbols
            (
                (new YCFunctionSignature<double>("sin", 1), Sin),
                (new YCFunctionSignature<double>("cos", 1), Cos),
                (new YCFunctionSignature<double>("f", 1), F)
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
            IYCAstBuilder builder = IYCAstBuilder.Instance;
            IYCInterpreter<double> interpreter = IYCInterpreter<double>.Make(new BasicNumberOperators.Double());
            IYCCompiler<double> compiler = IYCCompiler<double>.Make(new BasicNumberOperators.Double());
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
