using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using MarkusSecundus.YoowzxCalc.Numerics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using num = System.Double;

namespace MarkusSecundus.YoowzxCalc.Compilation.UnitTests
{
    internal class IdentifierValidatorTests
    {
        private static readonly string MockConstant = "__CONST", MockIdentifier = "__IDENTIFIER";
        private static string MockId(object rest) => MockIdentifier + rest;
        private class MockOperator : IYCNumberOperator<num>
        {
            public static readonly MockOperator Instance = new MockOperator();

            public double Add(double a, double b) => throw new NotImplementedException();
            public double Divide(double a, double b) => throw new NotImplementedException();
            public double IsEqual(double a, double b) => throw new NotImplementedException();
            public double IsLess(double a, double b) => throw new NotImplementedException();
            public double IsLessOrEqual(double a, double b) => throw new NotImplementedException();
            public bool IsTrue(double a) => throw new NotImplementedException();
            public double Modulo(double a, double b) => throw new NotImplementedException();
            public double Multiply(double a, double b) => throw new NotImplementedException();
            public double NegateLogical(double a) => throw new NotImplementedException();
            public double Power(double a, double power) => throw new NotImplementedException();
            public double Subtract(double a, double b) => throw new NotImplementedException();
            public double UnaryMinus(double a) => throw new NotImplementedException();

            public bool TryParseConstant(string repr, out double value) { value = 9d; return repr == MockConstant; }

            public FormatException ValidateIdentifier(string identifier)
                => identifier.StartsWith(MockIdentifier) ? null : new FormatException(InvalidIdentifierMessage(identifier));
        }

        static string InvalidIdentifierMessage(string identifierName) => $"Invalid identifier: {identifierName}";

        static List<FormatException> validate(YCFunctionDefinition def) => YCIdentifierValidator<num>.Instance.Scan(def, MockOperator.Instance);

        static List<FormatException> validate(YCExpression e)
        {
            var args = new YCIdentifierValidatorArgs<num> { Op = MockOperator.Instance, Exceptions = new () };
            e.Accept(YCIdentifierValidator<num>.Instance, args);
            return args.Exceptions;
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



        [Test]
        public void Literal_GetsCheckedForConstantFirst()
        {
            var validationResult = validate(lit(MockConstant));
            CollectionAssert.IsEmpty(validationResult);
        }

        [Test]
        public void Literal_GetsValidated()
        {
            var validationResult = validate(lit(MockId(47894)));
            CollectionAssert.IsEmpty(validationResult);

            validationResult = validate(lit("wqerfd"));
            Assert.AreEqual(1, validationResult.Count);
        }

        [Test]
        public void UnaryOperator_PassesValidationToChild()
        {
            YCExpression e(string s) => un<YCUnaryMinusExpression>(un<YCUnaryLogicalNotExpression>(un<YCUnaryPlusExpression>(lit(s))));

            var validationResult = validate(e(MockId(47894)));
            CollectionAssert.IsEmpty(validationResult);

            validationResult = validate(e("wqerfd"));
            Assert.AreEqual(1, validationResult.Count);
        }

        [Test]
        public void BinaryOperator_PassesValidationToChild()
        {
            YCExpression m = lit(MockId(0));
            var es = new Func<string, YCExpression>[]
            {
                s => bin<YCAddExpression>(bin<YCLogicalAndExpression>(bin<YCExponentialExpression>(lit(s), m), m), m),
                s => bin<YCAddExpression>(bin<YCLogicalAndExpression>(bin<YCExponentialExpression>(m, lit(s)), m), m),
                s => bin<YCAddExpression>(bin<YCLogicalAndExpression>(m, bin<YCExponentialExpression>(lit(s), m)), m),
                s => bin<YCAddExpression>(bin<YCLogicalAndExpression>(m, bin<YCExponentialExpression>(m, lit(s))), m),
            };

            foreach(var e in es)
            {
                var validationResult = validate(e(MockId(47894)));
                CollectionAssert.IsEmpty(validationResult);

                validationResult = validate(e("wqerfd"));
                Assert.AreEqual(1, validationResult.Count);
            }
        }
        [Test]
        public void TernaryOperator_PassesValidationToChild()
        {
            YCExpression m = lit(MockId(0));
            var es = new Func<string, YCExpression>[] {
                s => cond(cond(cond(lit(s), m, m), m, m), m, m),
                s => cond(cond(cond(m, lit(s), m), m, m), m, m),
                s => cond(cond(cond(m, m, lit(s)), m, m), m, m),
                s => cond(cond(cond(m, m, m), lit(s), m), m, m),
                s => cond(cond(m, cond(lit(s), m, m), m), m, m),
                s => cond(cond(m, m, cond(lit(s), m, m)), m, m)
            };

            foreach(var e in es)
            {
                var validationResult = validate(e(MockId(47894)));
                CollectionAssert.IsEmpty(validationResult);

                validationResult = validate(e("wqerfd"));
                Assert.AreEqual(1, validationResult.Count);
            }
        }

        [Test]
        public void FunctionCall_PassesValidationToChild()
        {
            YCExpression f(params YCExpression[] e) => fnc(MockIdentifier, e);
            YCExpression m = lit(MockId(0));
            var es = new Func<string, YCExpression>[] {
                s => f(f(f(lit(s), m, m), m, m), m, m),
                s => f(f(f(m, lit(s), m), m, m), m, m),
                s => f(f(f(m, m, lit(s)), m, m), m, m),
                s => f(f(f(m, m, m), lit(s), m), m, m),
                s => f(f(m, f(lit(s), m, m), m), m, m),
                s => f(f(m, m, f(lit(s), m, m)), m, m)
            };

            foreach(var e in es)
            {
                var validationResult = validate(e(MockId(47894)));
                CollectionAssert.IsEmpty(validationResult);

                validationResult = validate(e("wqerfd"));
                Assert.AreEqual(1, validationResult.Count);
            }
        }

        [Test]
        public void FunctionDefinition_AnonymousNameNotValidated()
        {
            var validationResult = validate(def(lit(MockId(0)), name:null));
            CollectionAssert.IsEmpty(validationResult);
        }

        [Test]
        public void FunctionDefinition_NameGetsValidated()
        {
            var validationResult = validate(def(lit(MockId(0)), name: MockId(43223)));
            CollectionAssert.IsEmpty(validationResult);

            validationResult = validate(def(lit(MockId(0)), name: "řčš"));
            Assert.AreEqual(1, validationResult.Count);
        }

        [Test]
        public void FunctionCall_NameGetsValidated()
        {
            var validationResult = validate(fnc(name: MockId(43223)));
            CollectionAssert.IsEmpty(validationResult);

            validationResult = validate(fnc(name: "žčřšěfds"));
            Assert.AreEqual(1, validationResult.Count);
        }


        [Test]
        public void DuplicitArgNamesInFunctionDefinition_AreError()
        {
            var validationResult = validate(def(lit(MockIdentifier), args: new[] { MockId(1), MockId(1) }));
            Assert.AreEqual(1, validationResult.Count);

            validationResult = validate(def(lit(MockIdentifier), args: new[] { MockId(2), MockId(1), MockId(1) }));
            Assert.AreEqual(1, validationResult.Count);

            validationResult = validate(def(lit(MockIdentifier), args: new[] {MockId(1), MockId(2), MockId(1) }));
            Assert.AreEqual(1, validationResult.Count);

            validationResult = validate(def(lit(MockIdentifier), args: new[] {MockId(2), MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) , MockId(2), MockId(1) }));
            Assert.AreEqual(1, validationResult.Count);
        }
    }
}
