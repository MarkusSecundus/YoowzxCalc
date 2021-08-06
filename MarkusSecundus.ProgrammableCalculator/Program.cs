using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.Compiler;
using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.Compiler.Impl;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{

    public class Program
    {
        static readonly Func<double, double> Sin = Math.Sin, Cos = Math.Cos, F=x=>100*x;


        public static void Main() => test1();


        public static void test1()
        {
            IASTBuilder builder = IASTBuilder.Instance;
            IASTInterpreter<double> interpreter = new ASTInterpreter<double>(new INumberOperator.Double());
            IASTCompiler<double> compiler = new ASTCompiler<double, INumberOperator.Double>(new());
            var ctx = IASTFunctioncallContext.Make<double>().ResolveSymbols
            (
                (new FunctionSignature<double>("sin", 1), Sin),
                (new FunctionSignature<double>("cos", 1), Cos),
                (new FunctionSignature<double>("f", 1), F)
            );


            var tree = builder.Build("f(x) := sin(x)**2 + cos(x)**2");


            for(double x = 0; x < 7; x+=0.1)
            {
                double a, b;
                Write("{0} ", a=interpreter.Interpret(ctx, tree, x));
                Write(b = (double)compiler.Compile(ctx, tree).DynamicInvoke(x));
                WriteLine(a==b?"":" !");
            }
        }

    }
}
