namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{
    /// <summary>
    /// Node representing a logical and expression (<c>`x &amp;&amp; y`</c> in C-like languages)
    /// </summary>
    public sealed record YCLogicalAndExpression : YCBinaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "&";

        public override string ToString() => ToString_canonicalImpl();
    }
}
