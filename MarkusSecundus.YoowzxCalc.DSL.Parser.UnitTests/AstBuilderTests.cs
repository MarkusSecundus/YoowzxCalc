using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.UnitTests
{
    //For convenience assumes lexer to work correctly
    internal class AstBuilderTests
    {
        IYCAstBuilder Builder = IYCAstBuilder.Instance;

        YCFunctionDefinition bld(string s) => Builder.Build(s);

        YCFunctionDefinition def(YCExpression body, string name = null, string[] args = null, Dictionary<string, string> annot = null)
            => new YCFunctionDefinition
            {
                Body = body,
                Name = name ?? YCFunctionDefinition.AnonymousFunctionName,
                Annotations = annot ?? CollectionsUtils.EmptyDictionary<string, string>(),
                Arguments = args?? CollectionsUtils.EmptyList<string>()
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
        public void SimpleConstantParse()
        {
            Assert.AreEqual(bld("C"), def(lit("C")) );
            Assert.AreEqual(bld("1234.43"), def(lit("1234.43")) );
        }

        [Test]
        public void FunctionNameDefinition()
        {
            Assert.AreEqual(bld("ff_() := 1"), def(lit("1"), name: "ff_"));
        }

        [Test]
        public void BasicBinaryOperators()
        {
            test<YCAddExpression>("+");
            test<YCSubtractExpression>("-");
            test<YCMultiplyExpression>("*");
            test<YCDivideExpression>("/");
            test<YCModuloExpression>("%");
            test<YCExponentialExpression>("**");

            test<YCCompareGreaterOrEqualExpression>(">=");
            test<YCCompareGreaterThanExpression>(">");
            test<YCCompareLessOrEqualExpression>("<=");
            test<YCCompareLessThanExpression>("<");
            test<YCCompareIsEqualExpression>("=");
            test<YCCompareIsNotEqualExpression>("!=");

            test<YCLogicalAndExpression>("&");
            test<YCLogicalOrExpression>("|");

            void test<TExpr>(string s) where TExpr : YCBinaryExpression, new()
                => Assert.AreEqual(
                    bld($"123 {s} a"),
                    def(bin<TExpr>(lit("123"), lit("a")))
                );
        }
        [Test]
        public void BasicUnaryOperators()
        {
            test<YCUnaryLogicalNotExpression>("!");
            test<YCUnaryPlusExpression>("+");
            test<YCUnaryMinusExpression>("-");

            void test<TExpr>(string s) where TExpr : YCUnaryExpression, new()
                => Assert.AreEqual(
                    bld($"{s}a"),
                    def(un<TExpr>(lit("a")))
                );
        }

        [Test]
        public void BasicConditionalOperator()
        {
            Assert.AreEqual(
                bld("1 ? 2 : 3"),
                def(cond(lit("1"), lit("2"), lit("3")))
            );
        }

        [Test]
        public void BasicFunctioncall()
        {
            Assert.AreEqual(bld("f()"), def(fnc("f")));

            Assert.AreEqual(
                bld("f(a)"),
                def(fnc("f", lit("a")))
            );
            Assert.AreEqual(
                bld("f(a, b, c)"),
                def(fnc("f", lit("a"), lit("b"), lit("c")))
            );
            Assert.AreEqual(
                bld("1(a, b, c)"),
                def(fnc("1", lit("a"), lit("b"), lit("c")))
            );
            Assert.AreEqual(
                bld("'Toto je text'(a, b, c)"),
                def(fnc("'Toto je text'", lit("a"), lit("b"), lit("c")))
            );
        }





        [Test]
        public void ArithmeticOperatorsAssociativity()
        {
            test<YCAddExpression>("+");
            test<YCSubtractExpression>("-");
            test<YCMultiplyExpression>("*");
            test<YCDivideExpression>("/");
            test<YCModuloExpression>("%");

            Assert.AreEqual(    //exponential operator has opposite precedence than the others
                bld($"1**2**3"),
                def(bin<YCExponentialExpression>(lit("1"), bin<YCExponentialExpression>(lit("2"), lit("3"))))
            );

            test<YCCompareGreaterOrEqualExpression>(">=");
            test<YCCompareGreaterThanExpression>(">");
            test<YCCompareLessOrEqualExpression>("<=");
            test<YCCompareLessThanExpression>("<");
            test<YCCompareIsEqualExpression>("=");
            test<YCCompareIsNotEqualExpression>("!=");

            test<YCLogicalAndExpression>("&");
            test<YCLogicalOrExpression>("|");

            void test<TExpr>(string s) where TExpr: YCBinaryExpression, new()
                => Assert.AreEqual(
                    bld($"1{s}2{s}3"),
                    def(bin<TExpr>(bin<TExpr>(lit("1"), lit("2")), lit("3")))
                );
        }
        [Test]
        public void BinaryOperatorsPrecedence()
        {
            Assert.AreEqual(
                bld("1*2+3"),
                def(bin<YCAddExpression>(bin<YCMultiplyExpression>(lit("1"), lit("2")), lit("3")))
            );
            Assert.AreEqual(
                bld("1+2*3"),
                def(bin<YCAddExpression>(lit("1"),bin<YCMultiplyExpression>(lit("2"), lit("3"))))
            );
            Assert.AreEqual(
                bld("1*2/3"),
                def(bin<YCDivideExpression>(bin<YCMultiplyExpression>(lit("1"), lit("2")), lit("3")))
            );
            Assert.AreEqual(
                bld("1*2%3"),
                def(bin<YCModuloExpression>(bin<YCMultiplyExpression>(lit("1"), lit("2")), lit("3")))
            );
            Assert.AreEqual(
                bld("1+2<=3"),
                def(bin<YCCompareLessOrEqualExpression>(bin<YCAddExpression>(lit("1"), lit("2")), lit("3")))
            );
            Assert.AreEqual(
                bld("1*2>3"),
                def(bin<YCCompareGreaterThanExpression>(bin<YCMultiplyExpression>(lit("1"), lit("2")), lit("3")))
            );
            Assert.AreEqual(
                bld("a&2<=3"),
                def(bin<YCLogicalAndExpression>(lit("a"), bin<YCCompareLessOrEqualExpression>(lit("2"), lit("3"))))
            );
            Assert.AreEqual(
                bld("a|2&3"),
                def(bin<YCLogicalOrExpression>(lit("a"), bin<YCLogicalAndExpression>(lit("2"), lit("3"))))
            );
            Assert.AreEqual(
                bld("a&2|3"),
                def(bin<YCLogicalOrExpression>(bin<YCLogicalAndExpression>(lit("a"), lit("2")), lit("3")))
            );
        }

        [Test]
        public void UnaryOperatorsPrecedence()
        {
            Assert.AreEqual(
                bld("+-a"),
                def(un<YCUnaryPlusExpression>(un<YCUnaryMinusExpression>(lit("a"))))
            );
            Assert.AreEqual(
                bld("+!a + +b"),
                def(bin<YCAddExpression>(
                    un<YCUnaryPlusExpression>(un<YCUnaryLogicalNotExpression>(lit("a"))),
                    un<YCUnaryPlusExpression>(lit("b"))
                ))
            );
        }

        [Test]
        public void TernaryOperatorPrecedence()
        {

        }

        [Test]
        public void CommaInFunctioncallPrecedence()
        {

        }

        [Test]
        public void BracketsUltimatePrecedence()
        {

        }
    }
}
