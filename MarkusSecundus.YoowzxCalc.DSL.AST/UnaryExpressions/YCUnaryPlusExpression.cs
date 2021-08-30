namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions
{
    /// <summary>
    /// Node representing operation of unary plus (<c>`+x`</c> in C-like languages)
    /// </summary>
    public sealed record YCUnaryPlusExpression : YCUnaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "+";
    }
}
