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

namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnitTests
{
    internal class VisitorTests
    {
        public enum VisitorStates
        {
            None = -1,
            UnaryLogicalNot, UnaryMinus, UnaryPlus,
            Add, Subtract, Multiply, Modulo, Divide, Exponential,
            GreaterOrEqual, GreaterThan, LessOrEqual, LessThan, IsEqual, IsNotEqual,
            LogicalAnd, LogicalOr,
            Conditional, Functioncall,
            Literal,
            AnyUnary, AnyBinary, AnyPrimary,
            AnyExpression
        }


        public abstract class TestVisitorBase : YCVisitorBase<VisitorStates, VisitorStates>
        {
            private VisitorStates _testVariable = VisitorStates.None;
            public VisitorStates TestVariable { get => _testVariable; set { _testVariable = value; ++Counter; } }

            public int Counter { get; private set; } = 0; //to keep track that always only one visitor method got called
        }

        public class TestVisitor1 : TestVisitorBase
        {
            public override VisitorStates Visit(YCUnaryLogicalNotExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryLogicalNot;
            public override VisitorStates Visit(YCUnaryMinusExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryMinus;
            public override VisitorStates Visit(YCUnaryPlusExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryPlus;

            public override VisitorStates Visit(YCAddExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Add;
            public override VisitorStates Visit(YCSubtractExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Subtract;
            public override VisitorStates Visit(YCMultiplyExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Multiply;
            public override VisitorStates Visit(YCDivideExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Divide;
            public override VisitorStates Visit(YCModuloExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Modulo;
            public override VisitorStates Visit(YCExponentialExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Exponential;

            public override VisitorStates Visit(YCCompareGreaterOrEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.GreaterOrEqual;
            public override VisitorStates Visit(YCCompareGreaterThanExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.GreaterThan;
            public override VisitorStates Visit(YCCompareLessOrEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LessOrEqual;
            public override VisitorStates Visit(YCCompareLessThanExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LessThan;
            public override VisitorStates Visit(YCCompareIsEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.IsEqual;
            public override VisitorStates Visit(YCCompareIsNotEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.IsNotEqual;

            public override VisitorStates Visit(YCLogicalAndExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LogicalAnd;
            public override VisitorStates Visit(YCLogicalOrExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LogicalOr;
            public override VisitorStates Visit(YCConditionalExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Conditional;
            public override VisitorStates Visit(YCFunctioncallExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Functioncall;
            public override VisitorStates Visit(YCLiteralExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Literal;
        }

        private void test_doesPerformLeafActions(TestVisitorBase v)
        {
            int counter = 0;

            test<YCUnaryLogicalNotExpression>(VisitorStates.UnaryLogicalNot);
            test<YCUnaryMinusExpression>(VisitorStates.UnaryMinus);
            test<YCUnaryPlusExpression>(VisitorStates.UnaryPlus);

            test<YCAddExpression>(VisitorStates.Add);
            test<YCSubtractExpression>(VisitorStates.Subtract);
            test<YCMultiplyExpression>(VisitorStates.Multiply);
            test<YCDivideExpression>(VisitorStates.Divide);
            test<YCModuloExpression>(VisitorStates.Modulo);
            test<YCExponentialExpression>(VisitorStates.Exponential);

            test<YCCompareGreaterOrEqualExpression>(VisitorStates.GreaterOrEqual);
            test<YCCompareGreaterThanExpression>(VisitorStates.GreaterThan);
            test<YCCompareLessOrEqualExpression>(VisitorStates.LessOrEqual);
            test<YCCompareLessThanExpression>(VisitorStates.LessThan);
            test<YCCompareIsEqualExpression>(VisitorStates.IsEqual);
            test<YCCompareIsNotEqualExpression>(VisitorStates.IsNotEqual);

            test<YCLogicalAndExpression>(VisitorStates.LogicalAnd);
            test<YCLogicalOrExpression>(VisitorStates.LogicalOr);
            test<YCConditionalExpression>(VisitorStates.Conditional);
            test<YCFunctioncallExpression>(VisitorStates.Functioncall);
            test<YCLiteralExpression>(VisitorStates.Literal);

            void test<TExpr>(VisitorStates expectedState) where TExpr : YCExpression, new()
            {
                var e = new TExpr();
                e.Accept(v, default);
                Assert.AreEqual(expectedState, v.TestVariable);
                Assert.AreEqual(++counter, v.Counter);
            }
        }


        [Test]
        public void Visitor_DoesVisit()
        {
            var v = new TestVisitor1();
            test_doesPerformLeafActions(v);
        }



        public class TestVisitor2 : TestVisitorBase
        {
            public override VisitorStates Visit(YCBinaryExpression expr, VisitorStates ctx) => VisitorStates.AnyBinary;
            public override VisitorStates Visit(YCUnaryExpression expr, VisitorStates ctx) => VisitorStates.AnyUnary;
            public override VisitorStates Visit(YCPrimaryExpression expr, VisitorStates ctx) => VisitorStates.AnyPrimary;

            public override VisitorStates Visit(YCUnaryLogicalNotExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryLogicalNot;
            public override VisitorStates Visit(YCUnaryMinusExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryMinus;
            public override VisitorStates Visit(YCUnaryPlusExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.UnaryPlus;

            public override VisitorStates Visit(YCAddExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Add;
            public override VisitorStates Visit(YCSubtractExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Subtract;
            public override VisitorStates Visit(YCMultiplyExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Multiply;
            public override VisitorStates Visit(YCDivideExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Divide;
            public override VisitorStates Visit(YCModuloExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Modulo;
            public override VisitorStates Visit(YCExponentialExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Exponential;

            public override VisitorStates Visit(YCCompareGreaterOrEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.GreaterOrEqual;
            public override VisitorStates Visit(YCCompareGreaterThanExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.GreaterThan;
            public override VisitorStates Visit(YCCompareLessOrEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LessOrEqual;
            public override VisitorStates Visit(YCCompareLessThanExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LessThan;
            public override VisitorStates Visit(YCCompareIsEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.IsEqual;
            public override VisitorStates Visit(YCCompareIsNotEqualExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.IsNotEqual;

            public override VisitorStates Visit(YCLogicalAndExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LogicalAnd;
            public override VisitorStates Visit(YCLogicalOrExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.LogicalOr;
            public override VisitorStates Visit(YCConditionalExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Conditional;
            public override VisitorStates Visit(YCFunctioncallExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Functioncall;
            public override VisitorStates Visit(YCLiteralExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.Literal;
        }

        [Test]
        public void Visitor_MoreConcreteOneOverridesGeneralOne()
        {
            var v = new TestVisitor2();
            test_doesPerformLeafActions(v);
        }




        public class TestVisitor3 : TestVisitorBase
        {
            public override VisitorStates Visit(YCBinaryExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.AnyBinary;
            public override VisitorStates Visit(YCUnaryExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.AnyUnary;
            public override VisitorStates Visit(YCPrimaryExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.AnyPrimary;

            public override VisitorStates Visit(YCExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.AnyExpression;
        }

        [Test]
        public void Visitor_UseGeneralOneIfConcreteNotProvided()
        {
            var v = new TestVisitor3();
            int counter = 0;

            test<YCUnaryLogicalNotExpression>(VisitorStates.AnyUnary);
            test<YCUnaryMinusExpression>(VisitorStates.AnyUnary);
            test<YCUnaryPlusExpression>(VisitorStates.AnyUnary);

            test<YCAddExpression>(VisitorStates.AnyBinary);
            test<YCSubtractExpression>(VisitorStates.AnyBinary);
            test<YCMultiplyExpression>(VisitorStates.AnyBinary);
            test<YCDivideExpression>(VisitorStates.AnyBinary);
            test<YCModuloExpression>(VisitorStates.AnyBinary);
            test<YCExponentialExpression>(VisitorStates.AnyBinary);

            test<YCCompareGreaterOrEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareGreaterThanExpression>(VisitorStates.AnyBinary);
            test<YCCompareLessOrEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareLessThanExpression>(VisitorStates.AnyBinary);
            test<YCCompareIsEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareIsNotEqualExpression>(VisitorStates.AnyBinary);

            test<YCLogicalAndExpression>(VisitorStates.AnyBinary);
            test<YCLogicalOrExpression>(VisitorStates.AnyBinary);
            test<YCConditionalExpression>(VisitorStates.AnyExpression);
            test<YCFunctioncallExpression>(VisitorStates.AnyExpression);
            test<YCLiteralExpression>(VisitorStates.AnyPrimary);

            void test<TExpr>(VisitorStates state) where TExpr : YCExpression, new()
            {
                var e = new TExpr();
                e.Accept(v, default);
                Assert.AreEqual(state, v.TestVariable);
                Assert.AreEqual(++counter, v.Counter);
            }
        }

        public class TestVisitor4 : TestVisitorBase
        {
            public override VisitorStates Visit(YCExpression expr, VisitorStates ctx) => TestVariable = VisitorStates.AnyExpression;
        }

        [Test]
        public void Visitor_UseMostGeneralOneIfNoneOtherProvided()
        {
            var v = new TestVisitor4();
            int counter = 0;

            test<YCUnaryLogicalNotExpression>();
            test<YCUnaryMinusExpression>();
            test<YCUnaryPlusExpression>();

            test<YCAddExpression>();
            test<YCSubtractExpression>();
            test<YCMultiplyExpression>();
            test<YCDivideExpression>();
            test<YCModuloExpression>();
            test<YCExponentialExpression>();

            test<YCCompareGreaterOrEqualExpression>();
            test<YCCompareGreaterThanExpression>();
            test<YCCompareLessOrEqualExpression>();
            test<YCCompareLessThanExpression>();
            test<YCCompareIsEqualExpression>();
            test<YCCompareIsNotEqualExpression>();

            test<YCLogicalAndExpression>();
            test<YCLogicalOrExpression>();
            test<YCConditionalExpression>();
            test<YCFunctioncallExpression>();
            test<YCLiteralExpression>();

            void test<TExpr>() where TExpr : YCExpression, new()
            {
                var e = new TExpr();
                e.Accept(v, default);
                Assert.AreEqual(VisitorStates.AnyExpression, v.TestVariable);
                Assert.AreEqual(++counter, v.Counter);
            }
        }


        [Test]
        public void Visitor_ArgumentsGetPassed()
        {
            TestVisitorBase v = new TestVisitor1();

            test<YCUnaryLogicalNotExpression>(VisitorStates.UnaryLogicalNot);
            test<YCUnaryMinusExpression>(VisitorStates.UnaryMinus);
            test<YCUnaryPlusExpression>(VisitorStates.UnaryPlus);

            test<YCAddExpression>(VisitorStates.Add);
            test<YCSubtractExpression>(VisitorStates.Subtract);
            test<YCMultiplyExpression>(VisitorStates.Multiply);
            test<YCDivideExpression>(VisitorStates.Divide);
            test<YCModuloExpression>(VisitorStates.Modulo);
            test<YCExponentialExpression>(VisitorStates.Exponential);

            test<YCCompareGreaterOrEqualExpression>(VisitorStates.GreaterOrEqual);
            test<YCCompareGreaterThanExpression>(VisitorStates.GreaterThan);
            test<YCCompareLessOrEqualExpression>(VisitorStates.LessOrEqual);
            test<YCCompareLessThanExpression>(VisitorStates.LessThan);
            test<YCCompareIsEqualExpression>(VisitorStates.IsEqual);
            test<YCCompareIsNotEqualExpression>(VisitorStates.IsNotEqual);

            test<YCLogicalAndExpression>(VisitorStates.LogicalAnd);
            test<YCLogicalOrExpression>(VisitorStates.LogicalOr);
            test<YCConditionalExpression>(VisitorStates.Conditional);
            test<YCFunctioncallExpression>(VisitorStates.Functioncall);
            test<YCLiteralExpression>(VisitorStates.Literal);

            v = new TestVisitor3();

            test<YCUnaryLogicalNotExpression>(VisitorStates.AnyUnary);
            test<YCUnaryMinusExpression>(VisitorStates.AnyUnary);
            test<YCUnaryPlusExpression>(VisitorStates.AnyUnary);

            test<YCAddExpression>(VisitorStates.AnyBinary);
            test<YCSubtractExpression>(VisitorStates.AnyBinary);
            test<YCMultiplyExpression>(VisitorStates.AnyBinary);
            test<YCDivideExpression>(VisitorStates.AnyBinary);
            test<YCModuloExpression>(VisitorStates.AnyBinary);
            test<YCExponentialExpression>(VisitorStates.AnyBinary);

            test<YCCompareGreaterOrEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareGreaterThanExpression>(VisitorStates.AnyBinary);
            test<YCCompareLessOrEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareLessThanExpression>(VisitorStates.AnyBinary);
            test<YCCompareIsEqualExpression>(VisitorStates.AnyBinary);
            test<YCCompareIsNotEqualExpression>(VisitorStates.AnyBinary);

            test<YCLogicalAndExpression>(VisitorStates.AnyBinary);
            test<YCLogicalOrExpression>(VisitorStates.AnyBinary);
            test<YCConditionalExpression>(VisitorStates.AnyExpression);
            test<YCFunctioncallExpression>(VisitorStates.AnyExpression);
            test<YCLiteralExpression>(VisitorStates.AnyPrimary);


            v = new TestVisitor4();

            test<YCUnaryLogicalNotExpression>(VisitorStates.AnyExpression);
            test<YCUnaryMinusExpression>(VisitorStates.AnyExpression);
            test<YCUnaryPlusExpression>(VisitorStates.AnyExpression);

            test<YCAddExpression>(VisitorStates.AnyExpression);
            test<YCSubtractExpression>(VisitorStates.AnyExpression);
            test<YCMultiplyExpression>(VisitorStates.AnyExpression);
            test<YCDivideExpression>(VisitorStates.AnyExpression);
            test<YCModuloExpression>(VisitorStates.AnyExpression);
            test<YCExponentialExpression>(VisitorStates.AnyExpression);

            test<YCCompareGreaterOrEqualExpression>(VisitorStates.AnyExpression);
            test<YCCompareGreaterThanExpression>(VisitorStates.AnyExpression);
            test<YCCompareLessOrEqualExpression>(VisitorStates.AnyExpression);
            test<YCCompareLessThanExpression>(VisitorStates.AnyExpression);
            test<YCCompareIsEqualExpression>(VisitorStates.AnyExpression);
            test<YCCompareIsNotEqualExpression>(VisitorStates.AnyExpression);

            test<YCLogicalAndExpression>(VisitorStates.AnyExpression);
            test<YCLogicalOrExpression>(VisitorStates.AnyExpression);
            test<YCConditionalExpression>(VisitorStates.AnyExpression);
            test<YCFunctioncallExpression>(VisitorStates.AnyExpression);
            test<YCLiteralExpression>(VisitorStates.AnyExpression);

            void test<TExpr>(VisitorStates expectedState) where TExpr : YCExpression, new()
            {
                var e = new TExpr();
                var ret = e.Accept(v, default);
                Assert.AreEqual(expectedState, ret);
            }
        }


        public class TestVisitor5 : YCVisitorBase<string, string>
        {
            public override string Visit(YCExpression expr, string ctx) => ctx;
        }



        [Test]
        public void Visitor_ContextGetsPassed()
        {
            const string c = "rjewognfdlkvk dsn klsdf";
            var v = new TestVisitor5();

            var e = new YCCompareGreaterOrEqualExpression();
            var ret = e.Accept(v, c);
            Assert.AreEqual(c, ret);
        }
    }
}
