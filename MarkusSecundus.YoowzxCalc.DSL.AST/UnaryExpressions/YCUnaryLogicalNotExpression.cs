namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions
{
    public sealed class YCUnaryLogicalNotExpression : YCUnaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public override string Symbol => "!";
    }
}
