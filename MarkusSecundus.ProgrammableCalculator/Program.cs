using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;

using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{
    class Program
    {

        public static void Main()
        {
            var tree = IASTBuilder.Instance.Build(@"f(x):= 12.3e0 & 10");

            Console.WriteLine(tree);  
        }

    }
}
