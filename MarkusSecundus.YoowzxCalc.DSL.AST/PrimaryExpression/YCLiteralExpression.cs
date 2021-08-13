namespace MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression
{
    public sealed class YCLiteralExpression : YCPrimaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public string Value { get; init; }


        public override string ToString() => $"`{Value}`";

        protected override bool Equals_impl(object obj) => obj is YCLiteralExpression e && Value == e.Value;

        protected override int ComputeHashCode() => Value.GetHashCode();
    }
}
