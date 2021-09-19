namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{




    /// <summary>
    /// Node representing arithmetic exponentiation expression (<c>`x ** y`</c> in Python-like languages)
    /// </summary>
    public sealed record YCExponentialExpression : YCBinaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "^";

        public override string ToString() => ToString_canonicalImpl();
    }
}
