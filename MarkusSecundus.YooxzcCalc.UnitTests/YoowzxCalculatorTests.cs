using Antlr4.Runtime;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using num = System.Int32;

namespace MarkusSecundus.YooxzcCalc.UnitTests
{
    /// <summary>
    /// TODO: Write some actual tests
    /// </summary>
    internal class YoowzxCalculatorTests
    {
        public class MockAstBuilder : IYCAstBuilder
        {
            public YCFunctionDefinition Value;

            public YCFunctionDefinition Build(string source) => Value;
            public YCFunctionDefinition Build(Stream source) => Value;
            public YCFunctionDefinition Build(TextReader source) => Value;
            public YCFunctionDefinition Build(Lexer lexer) => Value;
        }

        public class MockCompiler : IYCCompiler<num>
        {
            public Delegate Value;

            public IYCCompilationResult<num> Compile(IYCCompilationContext<num> ctx, YCFunctionDefinition toCompile)
                => new MockResult { Value = Value };

            public class MockResult : IYCCompilationResult<num>
            {
                public Delegate Value;
                TDelegate IYCCompilationResult<num>.Finalize<TDelegate>() => (TDelegate)Value;
            }
        }

        public class MockContext : IYCInterpretationContext<num>
        {
            public IReadOnlyDictionary<YCFunctionSignature<num>, Delegate> Functions { get; set; }
        }

        public IYCAstBuilder AstBuilder(YCFunctionDefinition d) => new MockAstBuilder { Value = d };

        IYoowzxCalculator<num> make(YCFunctionDefinition expr, Delegate func, params (YCFunctionSignature<num>, Delegate)[] defs)
            => IYoowzxCalculator<num>.Make(
                astBuilder: new MockAstBuilder { Value = expr}, 
                compiler: new MockCompiler { Value = func}, 
                context: new MockContext
                {
                    Functions = new Dictionary<YCFunctionSignature<num>, Delegate>( defs.Select(d=>d.AsKV()))
                }
        );


        [Test]
        public void f()
        {
        }
    }
}
