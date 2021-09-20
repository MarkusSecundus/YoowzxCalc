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
using MarkusSecundus.YoowzxCalc.Numerics;
using static System.Console;

namespace MarkusSecundus.YoowzxCalc
{
    struct MyNumberType { }

    public static class Program
    {

        public static void Main()
        {
            IYCNumberOperator<MyNumberType> op = YCBasicNumberOperators.Get<MyNumberType>();
            IYCCompiler<MyNumberType> compiler = IYCCompiler<MyNumberType>.Make(op);

            IYCFunctioncallContext<MyNumberType> ctx = null;
            YCFunctionDefinition toCompile = null;
            IYCCompilationResult<MyNumberType> result = compiler.Compile(ctx, toCompile);

            var r = result.Finalize();
            var runnableResult = result.Finalize<Func<MyNumberType, MyNumberType>>();
        }

    }
}
