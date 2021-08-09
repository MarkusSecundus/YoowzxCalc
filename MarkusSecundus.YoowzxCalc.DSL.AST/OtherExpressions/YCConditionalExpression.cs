namespace MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions
{
    public sealed class YCConditionalExpression : YCExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public YCExpression Condition { get; init; }
        public YCExpression IfTrue { get; init; }
        public YCExpression IfFalse { get; init; }


        public override YCExpression this[int childIndex] => new[] { Condition, IfTrue, IfFalse }[childIndex];

        public override int Arity => 3;


        public override string ToString() => $"({Condition} ? {IfTrue} : {IfFalse})";

        protected override bool Equals_impl(object obj) => obj is YCConditionalExpression b && Equals(Condition, b.Condition) && Equals(IfTrue, b.IfTrue) && Equals(IfFalse, b.IfFalse);

        protected override int ComputeHashCode() => (Condition, IfTrue, IfFalse).GetHashCode();
    }
}
