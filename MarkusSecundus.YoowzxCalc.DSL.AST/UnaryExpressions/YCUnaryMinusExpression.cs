namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions
{
    public sealed record YCUnaryMinusExpression : YCUnaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "-";
    }
}
