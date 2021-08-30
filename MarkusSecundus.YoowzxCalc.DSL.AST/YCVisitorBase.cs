using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;


namespace MarkusSecundus.YoowzxCalc.DSL.AST
{


    /// <summary>
    /// Abstract base for expression visitors that provides some convenience functionality on top of bare <see cref="IYCVisitor{TRet, TContext}"/> contract.
    /// <para/>
    /// Contains additional Visit methods for abstract expression supertypes - every Visit method by default redirects to Visit method of supertype, Visit(<see cref="YCExpression"/>) throws a <see cref="NotImplementedException"/>.
    /// 
    /// </summary>
    /// <typeparam name="TRet">Result type of the visit.</typeparam>
    /// <typeparam name="TContext">Type for carrying additional data needed during the visit.</typeparam>
    public abstract class YCVisitorBase<TRet, TContext> : IYCVisitor<TRet, TContext>
    {
        public virtual TRet Visit(YCLiteralExpression expr, TContext ctx) => Visit((YCPrimaryExpression)expr, ctx);


        public virtual TRet Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);

        public virtual TRet Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual TRet Visit(YCAddExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCSubtractExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCMultiplyExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCDivideExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCModuloExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCExponentialExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual TRet Visit(YCFunctioncallExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);





        public virtual TRet Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual TRet Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);


        public virtual TRet Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual TRet Visit(YCLogicalAndExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual TRet Visit(YCLogicalOrExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual TRet Visit(YCConditionalExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);











        public virtual TRet Visit(YCPrimaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);

        public virtual TRet Visit(YCUnaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual TRet Visit(YCBinaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual TRet Visit(YCExpression expr, TContext ctx) => throw new NotImplementedException();
    }
}
