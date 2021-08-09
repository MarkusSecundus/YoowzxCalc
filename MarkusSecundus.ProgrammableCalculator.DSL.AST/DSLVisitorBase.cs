using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class DSLVisitorBase<T, TContext> : IDSLVisitor<T, TContext>
    {
        public virtual T Visit(DSLConstantExpression expr, TContext ctx) => Visit((DSLPrimaryExpression)expr, ctx);

        public virtual T Visit(DSLArgumentExpression expr, TContext ctx) => Visit((DSLPrimaryExpression)expr, ctx);


        public virtual T Visit(DSLUnaryMinusExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);

        public virtual T Visit(DSLUnaryPlusExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);


        public virtual T Visit(DSLAddExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLSubtractExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLMultiplyExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLDivideExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLModuloExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLExponentialExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual T Visit(DSLFunctioncallExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);





        public virtual T Visit(DSLUnaryLogicalNotExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);


        public virtual T Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLCompareGreaterThanExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLCompareLessOrEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLCompareLessThanExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);


        public virtual T Visit(DSLCompareIsEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLCompareIsNotEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual T Visit(DSLLogicalAndExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual T Visit(DSLLogicalOrExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual T Visit(DSLConditionalExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);











        public virtual T Visit(DSLPrimaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);

        public virtual T Visit(DSLUnaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);


        public virtual T Visit(DSLBinaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);


        public virtual T Visit(DSLExpression expr, TContext ctx) => throw new NotImplementedException();





    }
}
