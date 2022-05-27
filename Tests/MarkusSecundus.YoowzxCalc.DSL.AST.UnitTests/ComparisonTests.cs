using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnitTests
{
    internal class ComparisonTests
    {
        
        #region HELPERS
        private void AssertEqual<T>(T a, T b)
        {
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }


        YCExpression lit(string s) => new YCLiteralExpression { Value = s };

        YCExpression bin<TExpr>(YCExpression left, YCExpression right) where TExpr: YCBinaryExpression, new()
            => new TExpr { LeftChild = left, RightChild = right };
        YCExpression un<TExpr>(YCExpression child) where TExpr: YCUnaryExpression, new()
            => new TExpr { Child = child};

        YCExpression cond(YCExpression condition, YCExpression l, YCExpression r)
            => new YCConditionalExpression { Condition = condition, IfTrue = l, IfFalse = r };

        #endregion

        #region LITERALS_TESTS
        [Test]
        public void Literals_Equals_True_OnEqualContents()
        {
            YCExpression c1 = lit("a"), c2 = lit("a");

            AssertEqual(c1, c2);
            AssertEqual(c2, c1);
            AssertEqual(c1, c1);
            AssertEqual(c2, c2);
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
        #endregion

        #region BINARIES_TESTS

        [Test]
        public void Binaries_Equals_True_OnEqualTypeAndContents()
        {
            YCExpression e1 = bin<YCAddExpression>(lit("fdsfds"), lit("67843:!"));
            AssertEqual(e1, e1);
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
                => AssertEqual(bin<TExpr>(left.Item1, left.Item2), bin<TExpr>(right.Item1, right.Item2));
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
                where TExpr1 : YCBinaryExpression, new() where TExpr2 : YCBinaryExpression, new()
                => Assert.AreNotEqual(bin<TExpr1>(a1, a2), bin<TExpr2>(a1, a2));
        }

        [Test]
        public void Binaries_Equals_False_OnDifferentContents()
        {
            test<YCSubtractExpression>((lit("a"), lit("b")), (lit("a "), lit("b")));
            test<YCMultiplyExpression>((lit("arewrwe"), lit("     \t ")), (lit("arewrwe"), lit("")));
            test<YCDivideExpression>((lit(" "), lit(null)), (lit(""), lit(null)));
            test<YCModuloExpression>((lit(""), lit("b")), (lit(null), lit("b")));

            test<YCCompareGreaterOrEqualExpression>((lit("null"), lit(null)), (null, lit(null)));
            test<YCCompareGreaterThanExpression>((null, null), (null, lit("null")));
            test<YCCompareLessOrEqualExpression>((null, lit("")), (null, lit("s")));
            test<YCCompareLessThanExpression>((null, lit("   \n  ")), (null, lit("   \n ")));
            test<YCCompareIsEqualExpression>((null, lit("rewrewr")), (null, lit("rewrew")));
            test<YCCompareIsNotEqualExpression>((lit("tre"), null), (lit(""), null));

            test<YCLogicalAndExpression>((lit(""), null), (lit(null), null));
            test<YCLogicalOrExpression>((lit("a"), lit("b")), (lit("c"), lit("d")));

            void test<TExpr>((YCExpression, YCExpression) left, (YCExpression, YCExpression) right) where TExpr : YCBinaryExpression, new()
                => Assert.AreNotEqual(bin<TExpr>(left.Item1, left.Item2), bin<TExpr>(right.Item1, right.Item2));
        }

        #endregion

        #region UNARIES_TESTS

        [Test]
        public void Unaries_Equals_True_OnEqualTypeAndContents()
        {
            YCExpression e1 = un<YCUnaryPlusExpression>(lit("fdsfds"));
            AssertEqual(e1, e1);

            test<YCUnaryLogicalNotExpression>(lit("arew r"), lit("arew r"));
            test<YCUnaryPlusExpression>(lit(""), lit(""));
            test<YCUnaryMinusExpression>(lit(null), lit(null));

            void test<TExpr>(YCExpression left, YCExpression right) where TExpr : YCUnaryExpression, new()
                => AssertEqual(un<TExpr>(left), un<TExpr>(right));
        }

        [Test]
        public void Unaries_Equals_False_OnDifferentType()
        {
            test<YCUnaryLogicalNotExpression, YCUnaryMinusExpression>(lit("asdsa"));
            test<YCUnaryLogicalNotExpression, YCUnaryPlusExpression>(lit("ewqeww"));
            test<YCUnaryMinusExpression, YCUnaryPlusExpression>(lit(null));
            test<YCUnaryPlusExpression, YCUnaryMinusExpression>(lit(""));
            test<YCUnaryPlusExpression, YCUnaryLogicalNotExpression>(null);
            test<YCUnaryMinusExpression, YCUnaryLogicalNotExpression>(null);

            void test<TExpr1, TExpr2>(YCExpression child)
                where TExpr1 : YCUnaryExpression, new() where TExpr2 : YCUnaryExpression, new()
                => Assert.AreNotEqual(un<TExpr1>(child), un<TExpr2>(child));
        }

        [Test]
        public void Unaries_Equals_False_OnDifferentContents()
        {
            YCExpression e1 = un<YCUnaryPlusExpression>(lit("fdsfds"));
            AssertEqual(e1, e1);

            test<YCUnaryLogicalNotExpression>(lit("arew  r"), lit("arew r"));
            test<YCUnaryPlusExpression>(lit(""), lit("     "));
            test<YCUnaryMinusExpression>(null, lit(null));

            void test<TExpr>(YCExpression left, YCExpression right) where TExpr : YCUnaryExpression, new()
                => Assert.AreNotEqual(un<TExpr>(left), un<TExpr>(right));
        }


        #endregion

        #region CONDITIONAL_TESTS

        [Test]
        public void Conditional_Equals_True_OnEqualContents()
        {
            YCExpression e;
            
            e = new YCConditionalExpression();
            AssertEqual(e, e);

            e = new YCConditionalExpression { Condition = lit("a") };
            AssertEqual(e, e);

            e = cond( lit(""), lit(""), lit(null) );
            AssertEqual(e, e);

            AssertEqual( new YCConditionalExpression(), new YCConditionalExpression());

            AssertEqual( 
                new YCConditionalExpression { IfTrue = lit(null)}, 
                new YCConditionalExpression { IfTrue = lit(null)}
            );

            AssertEqual(
                cond(lit("dsa"), lit("we"), lit(" ")),
                cond(lit("dsa"), lit("we"), lit(" "))
            );
        }

        [Test]
        public void Conditional_Equals_False_OnNonEqualContents()
        {
            Assert.AreNotEqual( new YCConditionalExpression { Condition = lit("")}, new YCConditionalExpression { Condition = lit("e")});

            Assert.AreNotEqual( 
                new YCConditionalExpression { IfTrue = lit(null)}, 
                new YCConditionalExpression { IfTrue = lit("")}
            );

            Assert.AreNotEqual(
                cond(lit("dsa"), lit("we "), lit(" ")),
                cond(lit("dsa"), lit("we"), lit(" "))
            );

            Assert.AreNotEqual(
                cond(lit("  \t"), lit("we"), lit(" ")),
                cond(lit("  \t "), lit("we "), lit(" "))
            );
        }

        #endregion


        #region FUNCTIONCALL_TESTS

        [Test]
        public void Functioncall_Equals_True_OnEqualNameAndArgs()
        {
            YCFunctioncallExpression 
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } },
                e2 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") }.ToList() };

            AssertEqual(e1, e1);
            AssertEqual(e1, e2);
            AssertEqual(e2, e1);
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
                e1 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("bb") } },
                e2 = new YCFunctioncallExpression { Name = "f1", Arguments = new[] { lit("a"), lit("b") } };

            Assert.AreNotEqual(e1, e2);
            Assert.AreNotEqual(e2, e1);
        }

        #endregion



        #region FUNCTIONDEF_TESTS

        [Test]
        public void FunctionDefinition_Equals_True_OnEqualContents()
        {
            YCFunctionDefinition d;

            d = new YCFunctionDefinition();
            AssertEqual(d, d);

            d = new YCFunctionDefinition
            {
                Name = "f1",
                Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } },
                Arguments = new[] { "a", "b", "c" },
                Body = lit("a")
            };
            AssertEqual(d, d);


            AssertEqual(
                new YCFunctionDefinition(),
                new YCFunctionDefinition()
            );
            AssertEqual(
                new YCFunctionDefinition { Name = "f" },
                new YCFunctionDefinition { Name = "f" }
            );
            AssertEqual(
                new YCFunctionDefinition { Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } } },
                new YCFunctionDefinition { Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } }.ToImmutableDictionary() }
            );
            AssertEqual(
                new YCFunctionDefinition { Arguments = new[] { "a", "b", "c" }.ToImmutableArray() },
                new YCFunctionDefinition { Arguments = new[] { "a", "b", "c" }.ToList() }
            );
            AssertEqual(
                new YCFunctionDefinition { Body = lit("f") },
                new YCFunctionDefinition { Body = lit("f") }
            );
            AssertEqual(
                new YCFunctionDefinition
                {
                    Name = "f1",
                    Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } },
                    Arguments = new[] { "a", "b", "c" },
                    Body = lit("a")
                },
                new YCFunctionDefinition
                {
                    Name = "f1",
                    Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } },
                    Arguments = new[] { "a", "b", "c" },
                    Body = lit("a")
                }
            );
        }

        [Test]
        public void FunctionDefinition_Equals_False_OnDifferentContents()
        {
            Assert.AreNotEqual(
                new YCFunctionDefinition { Name = "f" },
                new YCFunctionDefinition { Name = "f " }
            );
            Assert.AreNotEqual(
                new YCFunctionDefinition { Annotations = new Dictionary<string, string> { { "cached", "c" }, { "dsa", "ewwee" } } },
                new YCFunctionDefinition { Annotations = new Dictionary<string, string> { { "cached", null } } }
            );
            Assert.AreNotEqual(
                new YCFunctionDefinition { Arguments = new[] { "a", "b" } },
                new YCFunctionDefinition { Arguments = new[] { "a", "b", "c" }.ToList() }
            );
            Assert.AreNotEqual(
                new YCFunctionDefinition { Body = lit("fa") },
                new YCFunctionDefinition { Body = lit("f") }
            );
            Assert.AreNotEqual(
                new YCFunctionDefinition
                {
                    Name = "f",
                    Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } },
                    Arguments = new[] { "a", "b", "c" },
                    Body = lit("a")
                },
                new YCFunctionDefinition
                {
                    Name = "f1",
                    Annotations = new Dictionary<string, string> { { "cached", null }, { "dsa", "ewwee" } },
                    Arguments = new[] { "a", "b", "c" },
                    Body = lit("a")
                }
            );
        }

        #endregion

    }
}
