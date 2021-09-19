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
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using static System.Console;

namespace MarkusSecundus.YoowzxCalc
{

    public static class Program
    {

        public static void Main()
        {
            YCFunctionDefinition.AnonymousFunctionName
            IYCAstBuilder bld = IYCAstBuilder.Instance;
            var tree = bld.Build("f(x) := 10 + 3*f(y, @'Toto je text''WE  ' + 10)");

            Console.WriteLine(tree);
            return;

            IYoowzxCalculator<double> calc = IYoowzxCalculator<double>.Make();

            calc.AddFunction<Func<double>>("Pi", () => 4)
                .AddFunction<Func<double, double>>("Sin", Math.Sin)
                .AddFunction<Func<double, double>>("Print", x=> { Console.WriteLine(x); return x; });

            calc.AddFunctions("fact(x, acc) := x<= 1? acc : fact(x-1, x*acc)", "fact(x):= fact(x, 1)");

            //for (int t = 0; t < 10; ++t)
                WriteLine(calc.Get<Func<double, double>>("fact")(80000));

        }

    }
}
