using MarkusSecundus.Util;

namespace MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression
{
    /// <summary>
    /// Node representing a constant or a variable defined by an arbitrary string value.
    /// <para/>
    /// Determining which of those two or if it even is valid at all is left up to the user to allow more flexibility in various usage contexts.
    /// </summary>
    public sealed record YCLiteralExpression : YCPrimaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        /// <summary>
        /// Value of the literal.
        /// </summary>
        public string Value { get; init; }


        /// <inheritdoc/>
        public override string ToString() => $"{Value}";

        /// <inheritdoc/>
        protected override int ComputeHashCode() => Value?.GetHashCode()??0;
    }
}
