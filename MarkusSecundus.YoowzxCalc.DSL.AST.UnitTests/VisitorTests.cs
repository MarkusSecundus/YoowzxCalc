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

        public class TestVisitor1 : YCVisitorBase<VisitorStates, int>
        {
            public VisitorStates TestVariable = VisitorStates.None;


            public override VisitorStates Visit(YCUnaryLogicalNotExpression expr, int ctx) => TestVariable = VisitorStates.UnaryLogicalNot;
            public override VisitorStates Visit(YCUnaryMinusExpression expr, int ctx) => TestVariable = VisitorStates.UnaryMinus;
            public override VisitorStates Visit(YCUnaryPlusExpression expr, int ctx) => TestVariable = VisitorStates.UnaryPlus;

            public override VisitorStates Visit(YCAddExpression expr, int ctx) => TestVariable = VisitorStates.Add;
            public override VisitorStates Visit(YCSubtractExpression expr, int ctx) => TestVariable = VisitorStates.Subtract;
            public override VisitorStates Visit(YCMultiplyExpression expr, int ctx) => TestVariable = VisitorStates.Multiply;
            public override VisitorStates Visit(YCDivideExpression expr, int ctx) => TestVariable = VisitorStates.Divide;
            public override VisitorStates Visit(YCModuloExpression expr, int ctx) => TestVariable = VisitorStates.Modulo;
            public override VisitorStates Visit(YCExponentialExpression expr, int ctx) => TestVariable = VisitorStates.Exponential;

            public override VisitorStates Visit(YCCompareGreaterOrEqualExpression expr, int ctx) => TestVariable = VisitorStates.GreaterOrEqual;
            public override VisitorStates Visit(YCCompareGreaterThanExpression expr, int ctx) => TestVariable = VisitorStates.GreaterThan;
            public override VisitorStates Visit(YCCompareLessOrEqualExpression expr, int ctx) => TestVariable = VisitorStates.LessOrEqual;
            public override VisitorStates Visit(YCCompareLessThanExpression expr, int ctx) => TestVariable = VisitorStates.LessThan;
            public override VisitorStates Visit(YCCompareIsEqualExpression expr, int ctx) => TestVariable = VisitorStates.IsEqual;
            public override VisitorStates Visit(YCCompareIsNotEqualExpression expr, int ctx) => TestVariable = VisitorStates.IsNotEqual;

            public override VisitorStates Visit(YCLogicalAndExpression expr, int ctx) => TestVariable = VisitorStates.LogicalAnd;
            public override VisitorStates Visit(YCLogicalOrExpression expr, int ctx) => TestVariable = VisitorStates.LogicalOr;
            public override VisitorStates Visit(YCConditionalExpression expr, int ctx) => TestVariable = VisitorStates.Conditional;
            public override VisitorStates Visit(YCFunctioncallExpression expr, int ctx) => TestVariable = VisitorStates.Functioncall;
            public override VisitorStates Visit(YCLiteralExpression expr, int ctx) => TestVariable = VisitorStates.Literal;
        }

        private void test_doesPerformLeafActions(IYCVisitor<VisitorStates, int> v, Func<VisitorStates> stateGetter)
        {
            YCExpression e;

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

            void test<TExpr>(VisitorStates state) where TExpr : YCExpression, new()
            {
                e = new TExpr();
                e.Accept(v, 0);
                Assert.AreEqual(stateGetter(), state);
            }
        }


        [Test]
        public void Visitor_DoesVisit()
        {
            var v = new TestVisitor1();
            test_doesPerformLeafActions(v, () => v.TestVariable);
        }



        public class TestVisitor2 : YCVisitorBase<VisitorStates, int>
        {
            public VisitorStates TestVariable = VisitorStates.None;


            public override VisitorStates Visit(YCBinaryExpression expr, int ctx) => VisitorStates.AnyBinary;
            public override VisitorStates Visit(YCUnaryExpression expr, int ctx) => VisitorStates.AnyUnary;
            public override VisitorStates Visit(YCPrimaryExpression expr, int ctx) => VisitorStates.AnyPrimary;

            public override VisitorStates Visit(YCUnaryLogicalNotExpression expr, int ctx) => TestVariable = VisitorStates.UnaryLogicalNot;
            public override VisitorStates Visit(YCUnaryMinusExpression expr, int ctx) => TestVariable = VisitorStates.UnaryMinus;
            public override VisitorStates Visit(YCUnaryPlusExpression expr, int ctx) => TestVariable = VisitorStates.UnaryPlus;

            public override VisitorStates Visit(YCAddExpression expr, int ctx) => TestVariable = VisitorStates.Add;
            public override VisitorStates Visit(YCSubtractExpression expr, int ctx) => TestVariable = VisitorStates.Subtract;
            public override VisitorStates Visit(YCMultiplyExpression expr, int ctx) => TestVariable = VisitorStates.Multiply;
            public override VisitorStates Visit(YCDivideExpression expr, int ctx) => TestVariable = VisitorStates.Divide;
            public override VisitorStates Visit(YCModuloExpression expr, int ctx) => TestVariable = VisitorStates.Modulo;
            public override VisitorStates Visit(YCExponentialExpression expr, int ctx) => TestVariable = VisitorStates.Exponential;

            public override VisitorStates Visit(YCCompareGreaterOrEqualExpression expr, int ctx) => TestVariable = VisitorStates.GreaterOrEqual;
            public override VisitorStates Visit(YCCompareGreaterThanExpression expr, int ctx) => TestVariable = VisitorStates.GreaterThan;
            public override VisitorStates Visit(YCCompareLessOrEqualExpression expr, int ctx) => TestVariable = VisitorStates.LessOrEqual;
            public override VisitorStates Visit(YCCompareLessThanExpression expr, int ctx) => TestVariable = VisitorStates.LessThan;
            public override VisitorStates Visit(YCCompareIsEqualExpression expr, int ctx) => TestVariable = VisitorStates.IsEqual;
            public override VisitorStates Visit(YCCompareIsNotEqualExpression expr, int ctx) => TestVariable = VisitorStates.IsNotEqual;

            public override VisitorStates Visit(YCLogicalAndExpression expr, int ctx) => TestVariable = VisitorStates.LogicalAnd;
            public override VisitorStates Visit(YCLogicalOrExpression expr, int ctx) => TestVariable = VisitorStates.LogicalOr;
            public override VisitorStates Visit(YCConditionalExpression expr, int ctx) => TestVariable = VisitorStates.Conditional;
            public override VisitorStates Visit(YCFunctioncallExpression expr, int ctx) => TestVariable = VisitorStates.Functioncall;
            public override VisitorStates Visit(YCLiteralExpression expr, int ctx) => TestVariable = VisitorStates.Literal;
        }

        [Test]
        public void Visitor_MoreConcreteOneOverridesGeneralOne()
        {
            var v = new TestVisitor2();
            test_doesPerformLeafActions(v, () => v.TestVariable);
        }
    }
}
