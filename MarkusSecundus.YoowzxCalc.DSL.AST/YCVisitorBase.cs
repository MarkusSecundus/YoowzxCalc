using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCVisitorBase<T, TContext> : IYCVisitor<T, TContext>
    {
        public virtual T Visit(YCConstantExpression expr, TContext ctx) => Visit((YCPrimaryExpression)expr, ctx);

        public virtual T Visit(YCArgumentExpression expr, TContext ctx) => Visit((YCPrimaryExpression)expr, ctx);


        public virtual T Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);

        public virtual T Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual T Visit(YCAddExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCSubtractExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCMultiplyExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCDivideExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCModuloExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCExponentialExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual T Visit(YCFunctioncallExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);





        public virtual T Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual T Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);


        public virtual T Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual T Visit(YCLogicalAndExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual T Visit(YCLogicalOrExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual T Visit(YCConditionalExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);











        public virtual T Visit(YCPrimaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);

        public virtual T Visit(YCUnaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual T Visit(YCBinaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual T Visit(YCExpression expr, TContext ctx) => throw new NotImplementedException();





    }
}
