using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics.Impl;
using MarkusSecundus.ProgrammableCalculator.Parser;
using MarkusSecundus.Util;
using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{
    public class Program
    {
        public static void Main() => test1();

        public static void test1()
        {
        }

    }
}
