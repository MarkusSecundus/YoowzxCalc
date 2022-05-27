using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using NUnit.Framework;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnitTests
{
    internal class ComparisonTests
    {


        YCExpression lit(string s) => new YCLiteralExpression { Value = s };

        YCExpression bin<TExpr>(YCExpression left, YCExpression right) where TExpr: YCBinaryExpression, new()
            => new TExpr { LeftChild = left, RightChild = right };
        YCExpression un<TExpr>(YCExpression child) where TExpr: YCUnaryExpression, new()
            => new TExpr { Child = child};

        [Test]
        public void Literals_Equals_True_OnEqualContents()
        {
            YCExpression c1 = lit("a"), c2 = lit("a");

            Assert.AreEqual(c1, c2);
            Assert.AreEqual(c2, c1);
            Assert.AreEqual(c1, c1);
            Assert.AreEqual(c2, c2);
        }

        [Test]
        public void Literals_Equals_False_OnNonEqualContents()
        {
            Assert.AreNotEqual(lit("a"), lit("b"));
            Assert.AreNotEqual(lit("a"), lit("a "));
            Assert.AreNotEqual(lit(" a"), lit("a"));
            Assert.AreNotEqual(lit(""), lit(" "));
            Assert.AreNotEqual(lit("a"), lit("aa"));
            Assert.AreNotEqual(lit(null), lit(""));
            Assert.AreNotEqual(lit(""), lit(null));
            Assert.AreNotEqual(lit("null"), lit(null));
        }


        [Test]
        public void Binaries_Equals_True_OnEqualTypeAndContents()
        {
            YCExpression e1 = bin<YCAddExpression>(lit("fdsfds"), lit("67843:!"));
            Assert.AreEqual(e1, e1);
            test<YCSubtractExpression>((lit("a"), lit("b")), (lit("a"), lit("b")));
            test<YCMultiplyExpression>((lit("arewrwe"), lit("")), (lit("arewrwe"), lit("")));
            test<YCDivideExpression>((lit(""), lit(null)), (lit(""), lit(null)));
            test<YCModuloExpression>((lit(null), lit("b")), (lit(null), lit("b")));

            test<YCCompareGreaterOrEqualExpression>((null, lit(null)), (null, lit(null)));
            test<YCCompareGreaterThanExpression>((null, null), (null, null));
            test<YCCompareLessOrEqualExpression>((null, lit("")), (null, lit("")));
            test<YCCompareLessThanExpression>((null, lit("   \n ")), (null, lit("   \n ")));
            test<YCCompareIsEqualExpression>((null, lit("rewrew")), (null, lit("rewrew")));
            test<YCCompareIsNotEqualExpression>((lit(""), null), (lit(""), null));

            test<YCLogicalAndExpression>((lit(null), null), (lit(null), null));
            test<YCLogicalOrExpression>((lit(null), lit(null)), (lit(null), lit(null)));

            void test<TExpr>((YCExpression, YCExpression) left, (YCExpression, YCExpression) right) where TExpr : YCBinaryExpression, new()
                => Assert.AreEqual(bin<TExpr>(left.Item1, left.Item2), bin<TExpr>(right.Item1, right.Item2));
        }

        [Test]
        public void Binaries_Equals_False_OnDifferentType()
        {
            test<YCAddExpression, YCSubtractExpression>(lit("asdsa"), lit("ewqeww"));
            test<YCSubtractExpression, YCModuloExpression>(lit("asdsa"), lit("ewqeww"));
            test<YCMultiplyExpression, YCDivideExpression>(lit(""), lit(null));
            test<YCCompareGreaterOrEqualExpression, YCLogicalAndExpression>(lit("rew"), lit("reree"));
            test<YCCompareGreaterOrEqualExpression, YCCompareIsNotEqualExpression>(lit("rew"), null);
            test<YCCompareIsNotEqualExpression, YCCompareLessOrEqualExpression>(null, lit(null));
            test<YCCompareIsNotEqualExpression, YCCompareLessThanExpression>(lit(null), lit(null));
            test<YCCompareIsEqualExpression, YCLogicalOrExpression>(null, null);

            void test<TExpr1, TExpr2>(YCExpression a1, YCExpression a2) 
                where TExpr1 : YCBinaryExpression, new() where TExpr2: YCBinaryExpression, new()
                => Assert.AreNotEqual(bin<TExpr1>(a1, a2), bin<TExpr2>(a1, a2));
        }


        [Test]
        public void Functioncall_Equals_True_OnEqualNameAndArgs()
        {
            YCFunctioncallExpression 
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } },
                e2 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") }.ToList() };

            Assert.AreEqual(e1, e1);
            Assert.AreEqual(e1, e2);
            Assert.AreEqual(e2, e1);
        }


        [Test]
        public void Functioncall_Equals_False_OnDifferentName()
        {
            YCFunctioncallExpression
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } },
                e2 = new YCFunctioncallExpression { Name = "f2", Arguments = new[] { lit("a"), lit("b") } };

            Assert.AreNotEqual(e1, e2);
            Assert.AreNotEqual(e2, e1);
        }

        [Test]
        public void Functioncall_Equals_False_OnDifferentArgsCount()
        {
            YCFunctioncallExpression
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b"), lit("b") } },
                e2 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } };

            Assert.AreNotEqual(e1, e2);
            Assert.AreNotEqual(e2, e1);
        }

        [Test]
        public void Functioncall_Equals_False_OnDifferentArgs()
        {
            YCFunctioncallExpression
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("bb")} },
                e2 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } };

            Assert.AreNotEqual(e1, e2);
            Assert.AreNotEqual(e2, e1);
        }
    }
}
