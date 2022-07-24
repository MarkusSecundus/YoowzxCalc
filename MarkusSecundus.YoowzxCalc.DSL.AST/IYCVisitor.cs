using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;

namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    /// <summary>
    /// Object that can visit <see cref="YCExpression"/> and perform some operation on it.
    /// 
    /// <para>
    /// For more convenient-to-use abstract base classes see:
    /// <para/>
    /// - <see cref="YCVisitorBase{TRet, TContext}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoContext{TRet, TContext}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoReturn{TRet, TContext}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoReturnNoContext{TRet, TContext}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoContext{TRet}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoReturn{TContext}"/>
    /// <para/>
    /// - <see cref="YCVisitorBaseNoReturnNoContext"/>
    /// </para>
    /// <para>
    /// See: <see href="https://en.wikipedia.org/wiki/Visitor_pattern"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TRet">Result type of the visit.</typeparam>
    /// <typeparam name="TContext">Type for carrying additional information needed during the visit.</typeparam>
    public interface IYCVisitor<out TRet, in TContext>
    {
        /// <summary>
        /// Visit a <see cref="YCLiteralExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCLiteralExpression expr, TContext ctx);

        /// <summary>
        /// Visit a <see cref="YCUnaryMinusExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCUnaryMinusExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCUnaryPlusExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCUnaryPlusExpression expr, TContext ctx);

        /// <summary>
        /// Visit a <see cref="YCAddExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCAddExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCSubtractExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCSubtractExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCMultiplyExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCMultiplyExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCDivideExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCDivideExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCModuloExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCModuloExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCExponentialExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCExponentialExpression expr, TContext ctx);


        /// <summary>
        /// Visit a <see cref="YCFunctioncallExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCFunctioncallExpression expr, TContext ctx);



        /// <summary>
        /// Visit a <see cref="YCUnaryLogicalNotExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCUnaryLogicalNotExpression expr, TContext ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareGreaterOrEqualExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCCompareGreaterThanExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareGreaterThanExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCCompareLessOrEqualExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareLessOrEqualExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCCompareLessThanExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareLessThanExpression expr, TContext ctx);

        /// <summary>
        /// Visit a <see cref="YCCompareIsEqualExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareIsEqualExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCCompareIsNotEqualExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCCompareIsNotEqualExpression expr, TContext ctx);


        /// <summary>
        /// Visit a <see cref="YCLogicalAndExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCLogicalAndExpression expr, TContext ctx);
        /// <summary>
        /// Visit a <see cref="YCLogicalOrExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCLogicalOrExpression expr, TContext ctx);

        /// <summary>
        /// Visit a <see cref="YCConditionalExpression"/>
        /// </summary>
        /// <param name="expr">Node to be visited</param>
        /// <param name="ctx">Context for the visit</param>
        /// <returns>Result of the visit</returns>
        public TRet Visit(YCConditionalExpression expr, TContext ctx);

    }
}
