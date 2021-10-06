using BenchmarkDotNet.Running;
using System;

namespace MarkusSecundus.YoowzxCalc.Benchmarks
{
    public class Benchmarks
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ExpressionBenchmarks>();
        }
    }
}
