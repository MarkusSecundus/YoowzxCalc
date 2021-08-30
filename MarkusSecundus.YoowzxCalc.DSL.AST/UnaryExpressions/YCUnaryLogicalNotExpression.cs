namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions
{

    /// <summary>
    /// Node representing operation of logical negation (<c>`!x`</c> in C-like languages)
    /// </summary>
    public sealed record YCUnaryLogicalNotExpression : YCUnaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "!";
    }
}
