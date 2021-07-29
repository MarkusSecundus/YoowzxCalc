using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MarkusSecundus.ProgrammableCalculator.Numerics.Impl;
using System;
using System.Linq.Expressions;

namespace MarkusSecundus.ProgrammableCalculator.Benchmarks
{
    public class SimpleMathBenchmarks
    {
        static IExpressionEvaluator<DoubleNumber> eval = MarkusSecundus.ProgrammableCalculator.Program.MakeExpressionEvaluator();

        Func<DoubleNumber, DoubleNumber> fibDynamic, fibStatic, fibLambda;

        Func<DoubleNumber, DoubleNumber> mathDynamic, mathStatic, mathLambda;

        private DoubleNumber _fib(DoubleNumber x) => x.Le(1).IsZero() ? _fib(x.Sub(1)).Add(_fib(x.Sub(2))) : x;

        private DoubleNumber _math(DoubleNumber x) => x.Mul(x).Add((x.Div(2).Add(132))).Sub(x.Mul(0.32));

        private static Func<DoubleNumber, DoubleNumber> make(Expression<Func<DoubleNumber, DoubleNumber>> e) => e.Compile();
        private static Func<DoubleNumber, DoubleNumber> parse(string e) => eval.Parse(e) as Func<DoubleNumber, DoubleNumber>;



        Func<DoubleNumber, DoubleNumber> constDyn = parse(@"f(x) := 10"), constLambda = make(x => 10);
        Func<DoubleNumber, DoubleNumber> addDyn = parse(@"f(x) := x+10"), addLambda = make(x => x.Add(10));
        Func<DoubleNumber, DoubleNumber> mulDyn = parse(@"f(x) := x*10"), mulLambda = make(x => x.Mul(10));

        static Func<DoubleNumber, DoubleNumber> factDyn = parse(@"f(x) := x <= 1 ? 1 : x*f(x-1)"), factLambda = make(x => x <= 1 ? 1 : x*factLambda(x-1));


        public SimpleMathBenchmarks()
        {

            fibDynamic = parse(@"fib(x) := x <= 1 ?x : fib(x-1) + fib(x-2)");
            fibLambda = make(x => x.Le(1).IsZero() ? fibLambda(x.Sub(1)).Add(fibLambda(x.Sub(2))) : x);
            fibStatic = _fib;

            mathDynamic = parse(@"f(x) := x*x + (x/2 + 132) - x*0.32");
            mathLambda = make(x => x.Mul(x).Add((x.Div(2).Add(132))).Sub(x.Mul(0.32)));
            mathStatic = _math;

        }

        private const int _Limit = 6;

        [Benchmark] public DoubleNumber constD() => constDyn(0);
        [Benchmark] public DoubleNumber constL() => constLambda(0);

        [Benchmark] public DoubleNumber addD() => addDyn(_Limit);
        [Benchmark] public DoubleNumber addL() => addLambda(_Limit);

        [Benchmark] public DoubleNumber mulD() => mulDyn(_Limit);
        [Benchmark] public DoubleNumber mulL() => mulLambda(_Limit);

        [Benchmark] public DoubleNumber factD() => factDyn(_Limit);
        [Benchmark] public DoubleNumber factL() => factLambda(_Limit);

        [Benchmark] public DoubleNumber fibDynamicBench() => fibDynamic(_Limit);
        [Benchmark] public DoubleNumber fibLambdaBench() => fibLambda(_Limit);
        [Benchmark] public DoubleNumber fibStaticBench() => fibStatic(_Limit);
        
        [Benchmark] public DoubleNumber mathDynamicBench() => mathDynamic(_Limit);
        [Benchmark] public DoubleNumber mathLambdaBench() => mathLambda(_Limit);
        [Benchmark] public DoubleNumber mathStaticBench() => mathStatic(_Limit);

    }


    class Program
    {

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleMathBenchmarks>();
        }
    }
}
