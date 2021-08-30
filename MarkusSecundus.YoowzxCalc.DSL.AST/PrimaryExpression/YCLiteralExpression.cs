namespace MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression
{
    public sealed record YCLiteralExpression : YCPrimaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public string Value { get; init; }


        public override string ToString() => $"`{Value}`";

        protected override int ComputeHashCode() => Value.GetHashCode();
    }
}
