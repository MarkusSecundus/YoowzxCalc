namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{
    public sealed record YCExponentialExpression : YCBinaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public override string Symbol => "^";
    }
}
