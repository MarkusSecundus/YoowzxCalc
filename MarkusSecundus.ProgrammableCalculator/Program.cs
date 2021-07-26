using System;
using System.IO;
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
using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{
    class Program
    {
        
        public static void Main()
        {
            var ast = IASTBuilder.Instance.Build(@"f(x, y) := x **2");
            Console.WriteLine(ast);
            var parser = new ASTParser<DoubleNumber>(DoubleNumber.ConstantParser.Instance);

            var result = ast.Accept(parser, default);

            Console.WriteLine((result as Expression<Func<DoubleNumber, DoubleNumber, DoubleNumber>>).Compile()(10, 12).ToString());
        }

        public static void tst()
        {
            Expression<Func<int, int, int>> eee = (x, y) => x + y;

            Expression q = ((BinaryExpression)eee.Body).Left;
            Console.WriteLine(q);
            Console.WriteLine(Expression.Parameter(typeof(int), "q"));


            DoubleNumber a = 32.32, b = 123.1;
            Func<DoubleNumber, DoubleNumber > add = default(DoubleNumber).Sub;
            var variable = Expression.RuntimeVariables(Expression.Parameter(typeof(DoubleNumber), "dsa"));
            
            var addition = Expression.Call(variable, add.Method, Expression.Constant(b));


            var e = Expression.Lambda<Func<DoubleNumber>>(addition);

            Console.WriteLine(e.Compile()().ToString());

            //Console.WriteLine(IASTBuilder.Instance.Build(@"f(x):= 12.3e0 & 10"));  
        }

    }
}
