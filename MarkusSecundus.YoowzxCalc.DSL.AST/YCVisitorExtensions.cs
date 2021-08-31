using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;

namespace MarkusSecundus.YoowzxCalc.DSL.AST
{

    /// <summary>
    /// Static class containing extension functions to make working with YC visitors more convenient.
    /// </summary>
    public static class YCVisitorExtensions
    {
        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <typeparam name="TRet">Result type of the visit.</typeparam>
        /// <typeparam name="TContext">Context type of the visit.</typeparam>
        /// <param name="self">The expression node being visited.</param>
        /// <param name="visitor">The visitor.</param>
        /// <param name="ctx">Context for the visit.</param>
        /// <returns>Result of the visit.</returns>
        public static void Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoReturn<TRet, TContext> visitor, TContext ctx)
            => self.Accept(visitor, ctx);

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <typeparam name="TRet">Result type of the visit.</typeparam>
        /// <typeparam name="TContext">Context type of the visit.</typeparam>
        /// <param name="self">The expression node being visited.</param>
        /// <param name="visitor">The visitor.</param>
        /// <param name="ctx">Context for the visit.</param>
        /// <returns>Result of the visit.</returns>
        public static TRet Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <typeparam name="TRet">Result type of the visit.</typeparam>
        /// <typeparam name="TContext">Context type of the visit.</typeparam>
        /// <param name="self">The expression node being visited.</param>
        /// <param name="visitor">The visitor.</param>
        /// <param name="ctx">Context for the visit.</param>
        /// <returns>Result of the visit.</returns>
        public static void Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoReturnNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);
    }
}
