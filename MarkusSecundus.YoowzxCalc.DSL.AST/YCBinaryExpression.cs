using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    /// <summary>
    /// Abstract base for any binary subexpression in abstract syntax tree of a YoowzxCalc expression.
    /// </summary>
    public abstract record YCBinaryExpression : YCExpression
    {
        internal YCBinaryExpression() { }

        /// <summary>
        /// Left subexpression
        /// </summary>
        public YCExpression LeftChild { get; init; }

        /// <summary>
        /// Right subexpression
        /// </summary>
        public YCExpression RightChild { get; init; }

        /// <inheritdoc/>
        public sealed override int Arity => 2;

        public sealed override YCExpression this[int childIndex]
            => childIndex == 0 ? LeftChild :
               childIndex == 1 ? RightChild :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");



        /// <summary>
        /// Symbol textually representing the operation this node performs
        /// </summary>
        internal abstract string Symbol { get; }

        /// <summary>
        /// Canonical implementation to which all subclasse's <c>ToString()</c> should redirect.  
        /// (Records for some reason always require explicit override of <c>ToString()</c> for subclasses)
        /// </summary>
        /// <returns>Binary expression's string representation</returns>
        protected string ToString_canonicalImpl() => $"({LeftChild} {Symbol} {RightChild})";

        /// <inheritdoc/>
        public override string ToString() => ToString_canonicalImpl();

        /// <inheritdoc/>
        protected override int ComputeHashCode() => (LeftChild, RightChild).GetHashCode();
    }
}
