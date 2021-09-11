using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using static System.Console;

namespace MarkusSecundus.YoowzxCalc
{

    public static class Program
    {

        public static void Main()
        {
            IYoowzxCalculator<double> calc = IYoowzxCalculator<double>.Make();

            calc.AddFunction<Func<double>>("Pi", () => 4)
                .AddFunction<Func<double, double>>("Sin", Math.Sin)
                .AddFunction<Func<double, double>>("Print", x=> { Console.WriteLine(x); return x; });

            calc.AddFunctions("Fib10 := fib(10)", "fib(x) := x<= 1 ? x : fib(x-1) + fib(x-2)", "fib(x) := 76823.7456", "Fib3 := fib(3)");
            for (int t=0;t<10;++t)
                Console.WriteLine(calc.Get<Func<double, double>>("fib")(t));
            WriteLine();
            WriteLine(calc.Get<Func<double>>("Fib10")());
            WriteLine(calc.Get<Func<double>>("Fib3")());
        }

    }
}
