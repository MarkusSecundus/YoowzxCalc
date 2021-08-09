namespace MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions
{
    public sealed class DSLUnaryPlusExpression : DSLUnaryExpression
    {
        public override T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public override string Symbol => "+";
    }
}
