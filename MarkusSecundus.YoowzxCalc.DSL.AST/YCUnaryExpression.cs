using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    /// <summary>
    /// Abstract base for any unary subexpression in abstract syntax tree of a YoowzxCalc expression.
    /// </summary>
    public abstract record YCUnaryExpression : YCExpression
    {
        internal YCUnaryExpression() { }

        /// <summary>
        /// The only child of this node.
        /// </summary>
        public YCExpression Child { get; init; }

        /// <inheritdoc/>
        public sealed override int Arity => 1;

        /// <inheritdoc/>
        public sealed override YCExpression this[int childIndex]
            => childIndex == 0 ? Child :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");


        /// <summary>
        /// Symbol textually representing the operation this node performs
        /// </summary>
        internal abstract string Symbol { get; }

        /// <inheritdoc/>
        protected string ToString_canonicalImpl() => $"({Symbol}{Child})";

        /// <inheritdoc/>
        public override string ToString() => ToString_canonicalImpl();

        /// <inheritdoc/>
        protected override int ComputeHashCode() => Child?.GetHashCode()??0;
    }
}
