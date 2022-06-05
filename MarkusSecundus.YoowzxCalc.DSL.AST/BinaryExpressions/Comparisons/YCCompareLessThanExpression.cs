namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{




    /// <summary>
    /// Node representing a less-than expression (<c>`x &lt; y`</c> in C-like languages)
    /// </summary>
    public sealed record YCCompareLessThanExpression : YCBinaryExpression
    {
        /// <inheritdoc/>
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "<";

        /// <inheritdoc/>
        public override string ToString() => ToString_canonicalImpl();
    }
}
