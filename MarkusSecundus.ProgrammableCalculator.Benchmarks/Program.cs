using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics.Impl;
using System;
using System.Linq.Expressions;

namespace MarkusSecundus.ProgrammableCalculator.Benchmarks
{
    public class SimpleMathBenchmarks
    {
        static IExpressionEvaluator<DoubleNumber> eval = MarkusSecundus.ProgrammableCalculator.Program.MakeExpressionEvaluatorFcall();
        static IExpressionEvaluator<DoubleNumber> evalOp = MarkusSecundus.ProgrammableCalculator.Program.MakeExpressionEvaluatorOp();

        Func<DoubleNumber, DoubleNumber> fibDynamic, fibStatic, fibLambda;

        Func<DoubleNumber, DoubleNumber> mathDynamic, mathDynamicOp, mathStatic, mathLambda, mathLambda2;

        private DoubleNumber _fib(DoubleNumber x) => x.Le(1).IsZero() ? _fib(x.Sub(1)).Add(_fib(x.Sub(2))) : x;

        private DoubleNumber _math(DoubleNumber x) => x.Mul(x).Add((x.Div(2).Add(132))).Sub(x.Mul(0.32));

        private static Func<DoubleNumber, DoubleNumber> make(Expression<Func<DoubleNumber, DoubleNumber>> e) => e.Compile();
        private static Func<DoubleNumber, DoubleNumber> parse(string e, IExpressionEvaluator<DoubleNumber> ev = null) => (ev??eval).Parse(IASTBuilder.Instance.Build(e)) as Func<DoubleNumber, DoubleNumber>;



        Func<DoubleNumber, DoubleNumber> constDyn = parse(@"f(x) := 10"), constLambda = make(x => 10);
        Func<DoubleNumber, DoubleNumber> addDyn = parse(@"f(x) := x+10"), addLambda = make(x => x.Add(10));
        Func<DoubleNumber, DoubleNumber> mulDyn = parse(@"f(x) := x*10"), mulLambda = make(x => x.Mul(10));

        static Func<DoubleNumber, DoubleNumber> factDyn = parse(@"f(x) := x <= 1 ? 1 : x*f(x-1)"), factLambda = make(x => !(x <= 1).IsZero() ? 1 : x*factLambda(x-1));


        public SimpleMathBenchmarks()
        {

            fibDynamic = parse(@"fib(x) := x <= 1 ?x : fib(x-1) + fib(x-2)");
            fibLambda = make(x => x.Le(1).IsZero() ? fibLambda(x.Sub(1)).Add(fibLambda(x.Sub(2))) : x);
            fibStatic = _fib;

            mathDynamic = parse(@"f(x) := x*x + (x/2 + 132) - x*0.32");
            mathDynamicOp = parse(@"f(x) := x*x + (x/2 + 132) - x*0.32", evalOp);
            mathLambda = make(x => x.Mul(x).Add((x.Div(2).Add(132))).Sub(x.Mul(0.32)));
            mathLambda2 = make(x => x * x + (x / 2 + 132) - x * 0.32);
            mathStatic = _math;

        }

        private const int _Limit = 6;

        /*[Benchmark] public DoubleNumber constD() => constDyn(0);
        [Benchmark] public DoubleNumber constL() => constLambda(0);

        [Benchmark] public DoubleNumber addD() => addDyn(_Limit);
        [Benchmark] public DoubleNumber addL() => addLambda(_Limit);

        [Benchmark] public DoubleNumber mulD() => mulDyn(_Limit);
        [Benchmark] public DoubleNumber mulL() => mulLambda(_Limit);

        [Benchmark] public DoubleNumber factD() => factDyn(_Limit);
        [Benchmark] public DoubleNumber factL() => factLambda(_Limit);

        [Benchmark] public DoubleNumber fibDynamicBench() => fibDynamic(_Limit);
        [Benchmark] public DoubleNumber fibLambdaBench() => fibLambda(_Limit);
        [Benchmark] public DoubleNumber fibStaticBench() => fibStatic(_Limit);*/
        
        [Benchmark] public DoubleNumber mathDynamicBench() => mathDynamic(_Limit);
        [Benchmark] public DoubleNumber mathDynamicOpBench() => mathDynamic(_Limit);
        [Benchmark] public DoubleNumber mathLambdaBench() => mathLambda(_Limit);
        [Benchmark] public DoubleNumber mathLambdaOpBench() => mathLambda2(_Limit);
        [Benchmark] public DoubleNumber mathStaticBench() => mathStatic(_Limit);

    }

    public class ManualBenchmarks
    {
        public static Expression<Func<int, int>> m1 = x => x * x + (x / 2 + 132) - x * 32;
        public static Expression<Func<int, int>> m2 = makeM2();

        public Func<int, int> c1 = m1.Compile(), c2 = m2.Compile();

        static Expression Const(object o) => Expression.Constant(o);

        static Expression<Func<int, int>> makeM2()
        {
            var param = Expression.Parameter(typeof(int), "x");
            return Expression.Lambda<Func<int, int>>(
                Expression.Subtract(
                    Expression.Add(
                        Expression.Multiply(param, param),
                        Expression.Add(Expression.Divide(param, Const(2)), Const(132))
                    ),
                    Expression.Multiply(param, Const(32))
                ),
                true,
                param
           );
        }

        [Benchmark] public int lambda() => m1.Compile()(10);
        [Benchmark] public int dynamic() => m2.Compile()(10);


    }


    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine(ManualBenchmarks.m1);
            Console.WriteLine();
            Console.WriteLine(ManualBenchmarks.m2);
            var summary = BenchmarkRunner.Run<SimpleMathBenchmarks>();
        }
    }
}
