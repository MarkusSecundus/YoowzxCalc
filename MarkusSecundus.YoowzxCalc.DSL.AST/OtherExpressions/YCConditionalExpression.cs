namespace MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions
{

    /// <summary>
    /// Node representing conditional ternary operator (<c>`x?y:z`</c> in C-like languages).
    /// </summary>
    public sealed record YCConditionalExpression : YCExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        /// <summary>
        /// Subexpression representing the condition.
        /// </summary>
        public YCExpression Condition { get; init; }

        /// <summary>
        /// Subexpression to be performed if Condition is <c>true</c>
        /// </summary>
        public YCExpression IfTrue { get; init; }
        /// <summary>
        /// Subexpression to be performed if Condition is <c>false</c>
        /// </summary>
        public YCExpression IfFalse { get; init; }


        public override YCExpression this[int childIndex] => new[] { Condition, IfTrue, IfFalse }[childIndex];

        public override int Arity => 3;


        public override string ToString() => $"({Condition} ? {IfTrue} : {IfFalse})";

        protected override int ComputeHashCode() => (Condition, IfTrue, IfFalse).GetHashCode();
    }
}
