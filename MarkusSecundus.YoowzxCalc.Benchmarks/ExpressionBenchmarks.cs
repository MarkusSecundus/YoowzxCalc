using BenchmarkDotNet.Attributes;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.YoowzxCalc.Benchmarks
{
    public class ExpressionBenchmarks
    {
        private readonly IYoowzxCalculator<long> calc = IYoowzxCalculator<long>.Make();

        private readonly Func<long, long> fibonacci_yc, fibonacci_cs;
        private readonly Func<long, long> fibonacci_cached_yc, fibonacci_cached_cs;

        private readonly Func<long, long, long> factorial_yc, factorial_cs;


        private readonly Func<long, long, long, long, long, long, long> sum_yc, sum_cs;

        public ExpressionBenchmarks()
        {
            fibonacci_yc = calc.Compile<Func<long, long>>(@"fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");
            fibonacci_cs = x => x <= 1 ? x : fibonacci_cs(x - 1) + fibonacci_cs(x - 2);

            factorial_yc = calc.Compile<Func<long, long, long>>(@"fact(x, acc) := x <= 1 ? acc : fact(x-1, acc*x)");
            factorial_cs = (x, acc) => x <= 1 ? acc : factorial_cs(x - 1, acc * x);

            sum_yc = calc.Compile<Func<long, long, long, long, long, long, long>>(@"f(a, b, c, d, e, f) := a + b + c + d + e + f");
            sum_cs = (a, b, c, d, e, f) => a + b + c + d + e + f;


            fibonacci_cached_yc = calc.Compile<Func<long, long>>(@"[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)");
            fibonacci_cached_cs = ((Func<long, long>)(x => x <= 1 ? x : fibonacci_cached_cs(x - 1) + fibonacci_cached_cs(x - 2))).Autocached();
        }



        [Benchmark]
        public long Fibonacci_Yoowzx()
            => fibonacci_yc(25);

        [Benchmark]
        public long Fibonacci_CSharp()
            => fibonacci_cs(25);


        [Benchmark]
        public long Fibonacci_Cached_Yoowzx()
        {
            long ret = 0;
            for (long t = 100; t < 50000; ++t)
                ret += fibonacci_cached_yc(t);
            return ret;
        }

        [Benchmark]
        public long Fibonacci_Cached_CSharp()
        {
            long ret = 0;
            for (long t = 100; t < 50000; ++t)
                ret += fibonacci_cached_cs(t);
            return ret;
        }


        [Benchmark]
        public long Factorial_Yoowzx()
        {
            long ret = 0;
            for (long t = 0; t < 50; ++t)
                ret += factorial_yc(t*100, 1);
            return ret;
        }

        [Benchmark]
        public long Factorial_CSharp()
        {
            long ret = 0;
            for (long t = 0; t < 50; ++t)
                ret += factorial_cs(t*100, 1);
            return ret;
        }

        [Benchmark]
        public long Sum_Yoowzx()
        {
            const long MAX = 10;
            long ret = 0;
            for (long a = MAX; --a >= 0;)
                for (long b = MAX; --b >= 0;)
                    for (long c = MAX; --c >= 0;)
                        for (long d = MAX; --d >= 0;)
                            for (long e = MAX; --e >= 0;)
                                for (long f = MAX; --f >= 0;)
                                    ret += sum_yc(a, b, c, d, e, f);
            return ret;
        }
        [Benchmark]
        public long Sum_CSharp()
        {
            const long MAX = 10;
            long ret = 0;
            for (long a = MAX; --a >= 0;)
                for (long b = MAX; --b >= 0;)
                    for (long c = MAX; --c >= 0;)
                        for (long d = MAX; --d >= 0;)
                            for (long e = MAX; --e >= 0;)
                                for (long f = MAX; --f >= 0;)
                                    ret += sum_cs(a, b, c, d, e, f);
            return ret;
        }
    }
}
