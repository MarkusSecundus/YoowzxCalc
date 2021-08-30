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
    /// </summary>
    /// <typeparam name="TRet">Result type of the visit.</typeparam>
    public abstract class YCVisitorBaseNoContext<TRet> : YCVisitorBaseNoContext<TRet, object> { }

    /// <summary>
    /// Abstract base for expression visitors that provides some convenience functionality on top of bare <see cref="IYCVisitor{TRet, TContext}"/> contract.
    /// <para/>
    /// Contains additional Visit methods for abstract expression supertypes - every Visit method by default redirects to Visit method of supertype, Visit(<see cref="YCExpression"/>) throws a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <typeparam name="TRet">Result type of the visit.</typeparam>
    public abstract class YCVisitorBaseNoContext<TRet, TContext> : IYCVisitor<TRet, TContext>
    {
        public virtual TRet Visit(YCLiteralExpression expr) => Visit((YCPrimaryExpression)expr);


        public virtual TRet Visit(YCUnaryMinusExpression expr) => Visit((YCUnaryExpression)expr);

        public virtual TRet Visit(YCUnaryPlusExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual TRet Visit(YCAddExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCSubtractExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCMultiplyExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCDivideExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCModuloExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCExponentialExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual TRet Visit(YCFunctioncallExpression expr) => Visit((YCExpression)expr);





        public virtual TRet Visit(YCUnaryLogicalNotExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual TRet Visit(YCCompareGreaterOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCCompareGreaterThanExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCCompareLessOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCCompareLessThanExpression expr) => Visit((YCBinaryExpression)expr);


        public virtual TRet Visit(YCCompareIsEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCCompareIsNotEqualExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual TRet Visit(YCLogicalAndExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual TRet Visit(YCLogicalOrExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual TRet Visit(YCConditionalExpression expr) => Visit((YCExpression)expr);











        public virtual TRet Visit(YCPrimaryExpression expr) => Visit((YCExpression)expr);

        public virtual TRet Visit(YCUnaryExpression expr) => Visit((YCExpression)expr);


        public virtual TRet Visit(YCBinaryExpression expr) => Visit((YCExpression)expr);


        public virtual TRet Visit(YCExpression expr) => throw new NotImplementedException();




        TRet IYCVisitor<TRet, TContext>.Visit(YCLiteralExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCAddExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCSubtractExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCMultiplyExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCDivideExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCModuloExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCExponentialExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCFunctioncallExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalAndExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalOrExpression expr, TContext ctx) => Visit(expr);
        TRet IYCVisitor<TRet, TContext>.Visit(YCConditionalExpression expr, TContext ctx) => Visit(expr);
    }
}
