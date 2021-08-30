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
            var bld = IYCAstBuilder.Instance;
            var tree = bld.Build("černá + zelená - 45 + 45e-78");
            Console.WriteLine(tree);
        }

    }
}
