using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;

namespace MarkusSecundus.YoowzxCalc.DSL.AST
{


    /// <summary>
    /// Abstract base for any leaf node in abstract syntax tree of a YoowzxCalc expression.
    /// </summary>
    public abstract record YCPrimaryExpression : YCExpression
    {
        internal YCPrimaryExpression() { }

        /// <summary>
        /// 0 - Primary expression has no children
        /// </summary>
        public sealed override int Arity => 0;

        /// <summary>
        /// Always throws <see cref="IndexOutOfRangeException"/> - Primary expression has no children
        /// </summary>
        public sealed override YCExpression this[int childIndex]
            => throw new IndexOutOfRangeException($"Is a primary expression and therefore has 0 children");
    }
}
