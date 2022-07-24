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
    /// <typeparam name="TContext">Type for carrying additional data needed during the visit.</typeparam>
    public abstract class YCVisitorBaseNoReturn<TContext> : YCVisitorBaseNoReturn<object, TContext> { }

    /// <summary>
    /// Abstract base for expression visitors that provides some convenience functionality on top of bare <see cref="IYCVisitor{TRet, TContext}"/> contract.
    /// <para/>
    /// Contains additional Visit methods for abstract expression supertypes - every Visit method by default redirects to Visit method of supertype, Visit(<see cref="YCExpression"/>) throws a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <typeparam name="TContext">Type for carrying additional data needed during the visit.</typeparam>
    public abstract class YCVisitorBaseNoReturn<TRet, TContext> : IYCVisitor<TRet, TContext>
    {
        /// <summary>
        /// Visit a <see cref="YCLiteralExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCPrimaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCLiteralExpression expr, TContext ctx) => Visit((YCPrimaryExpression)expr, ctx);


        /// <summary>
        /// Visit a <see cref="YCUnaryMinusExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCUnaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCUnaryPlusExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCUnaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        /// <summary>
        /// Visit a <see cref="YCAddExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCAddExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCSubtractExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCSubtractExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCMultiplyExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCMultiplyExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCDivideExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCDivideExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCModuloExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCModuloExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCExponentialExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCExponentialExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        /// <summary>
        /// Visit a <see cref="YCFunctioncallExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCFunctioncallExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);





        /// <summary>
        /// Visit a <see cref="YCUnaryLogicalNotExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCUnaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        /// <summary>
        /// Visit a <see cref="YCCompareGreaterOrEqualExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareGreaterThanExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareLessOrEqualExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareLessThanExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);


        /// <summary>
        /// Visit a <see cref="YCCompareIsEqualExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareIsNotEqualExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        /// <summary>
        /// Visit a <see cref="YCLogicalAndExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCLogicalAndExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        /// <summary>
        /// Visit a <see cref="YCLogicalOrExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCBinaryExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCLogicalOrExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        /// <summary>
        /// Visit a <see cref="YCConditionalExpression"/>.
        /// If not provided, redirects to <see cref="Visit(YCExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCConditionalExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);











        /// <summary>
        /// Visit a generic <see cref="YCPrimaryExpression"/> if visit logic for the specific subclass is not provided.
        /// If not provided, redirects to <see cref="Visit(YCExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCPrimaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);

        /// <summary>
        /// Visit a generic <see cref="YCUnaryExpression"/> if visit logic for the specific subclass is not provided.
        /// If not provided, redirects to <see cref="Visit(YCExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCUnaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        /// <summary>
        /// Visit a generic <see cref="YCBinaryExpression"/> if visit logic for the specific subclass is not provided.
        /// If not provided, redirects to <see cref="Visit(YCExpression, TContext)"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCBinaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        /// <summary>
        /// Visit a generic <see cref="YCExpression"/> if more specific visitor logic is not provided.
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public virtual void Visit(YCExpression expr, TContext ctx) => throw new NotImplementedException();


        TRet IYCVisitor<TRet, TContext>.Visit(YCLiteralExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryMinusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryPlusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCAddExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCSubtractExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCMultiplyExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCDivideExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCModuloExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCExponentialExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCFunctioncallExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryLogicalNotExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsNotEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalAndExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalOrExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCConditionalExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
    }
}
