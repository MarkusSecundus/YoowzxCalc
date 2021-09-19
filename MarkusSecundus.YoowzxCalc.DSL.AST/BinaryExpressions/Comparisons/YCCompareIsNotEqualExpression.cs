namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{




    /// <summary>
    /// Node representing a non-equality comparison expression (<c>`x != y`</c> in C-like languages)
    /// </summary>
    public sealed record YCCompareIsNotEqualExpression : YCBinaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "!=";

        public override string ToString() => ToString_canonicalImpl();
    }
}
