namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{
    public sealed class DSLSubtractExpression : DSLBinaryExpression
    {
        public override T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public override string Symbol => "-";
    }
}
