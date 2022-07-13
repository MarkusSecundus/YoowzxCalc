using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using num = System.Int64;

namespace MarkusSecundus.YoowzxCalc.Compilation.UnitTests
{
    internal class CompilerBaseTests
    {
        class MockOperator : IYCNumberOperator<num>
        {
            public static readonly MockOperator Instance = new MockOperator();

            private num bo(bool b) => b ? 1 : 0;
            private bool bo(num b) => b == 0 ? false : true;

            public num Add(num a, num b) => a + b;
            public num Subtract(num a, num b) => a - b;
            public num Multiply(num a, num b) => a * b;
            public num Divide(num a, num b) => a / b;
            public num Modulo(num a, num b) => a % b;
            public num IsEqual(num a, num b) => bo(a == b);
            public num IsLess(num a, num b) => bo(a < b);
            public num IsLessOrEqual(num a, num b) => bo(a <= b);
            public bool IsTrue(num a) => bo(a);
            public num UnaryMinus(num a) => -a;
            public num UnaryPlus(num a) => num.MaxValue;

            public num Power(num a, num power) => throw new NotImplementedException();
            public num NegateLogical(num a) => bo(!bo(a));

            public bool TryParseConstant(string repr, out num value) => num.TryParse(repr, out value);
            public FormatException ValidateIdentifier(string identifier) => null;
        }

        readonly YCCompilerBase<num> compiler = new YCCompilerBase<num>(MockOperator.Instance);

        private const string SampleConstantName = "SampleConstant", SampleFunctionName = "SampleFunction";
        private ulong SampleConstantWasCalled, SampleFunctionWasCalled;

        readonly YCCompilationContext<num> compilationContext;

        public CompilerBaseTests()
        {
            compilationContext = new(YCCompilerUtils.MakeFunctionsAdder<num>()
                .Add<Func<num>>(SampleConstantName, () => { ++SampleConstantWasCalled; return num.MinValue; })
                .Add<Func<num, num, num>>(SampleFunctionName, (a, b) => { ++SampleFunctionWasCalled;return a + b; })
                .Value);
        }


        YCFunctionDefinition def(YCExpression body, string name = null, string[] args = null, Dictionary<string, string> annot = null)
            => new YCFunctionDefinition
            {
                Body = body,
                Name = name ?? YCFunctionDefinition.AnonymousFunctionName,
                Annotations = annot ?? YCFunctionDefinition.EmptyAnnotations,
                Arguments = args ?? YCFunctionDefinition.EmptyArguments
            };

        YCExpression lit(string s) => new YCLiteralExpression { Value = s };
        YCExpression bin<TExpr>(YCExpression left, YCExpression right) where TExpr : YCBinaryExpression, new()
            => new TExpr { LeftChild = left, RightChild = right };
        YCExpression un<TExpr>(YCExpression child) where TExpr : YCUnaryExpression, new()
            => new TExpr { Child = child };

        YCExpression cond(YCExpression condition, YCExpression l, YCExpression r)
            => new YCConditionalExpression { Condition = condition, IfTrue = l, IfFalse = r };

        YCExpression fnc(string name, params YCExpression[] args)
            => new YCFunctioncallExpression { Name = name, Arguments = args };


        Func<num> compile(YCExpression e) => compiler.Compile(compilationContext, def(e)).Finalize<Func<num>>();



    }
}
