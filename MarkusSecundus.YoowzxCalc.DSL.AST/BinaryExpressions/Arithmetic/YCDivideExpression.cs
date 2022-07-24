﻿namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{




    /// <summary>
    /// Node representing arithmetic division expression (<c>`x / y`</c> in C-like languages)
    /// </summary>
    public sealed record YCDivideExpression : YCBinaryExpression
    {
        /// <inheritdoc/>
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "/";

        /// <inheritdoc/>
        public override string ToString() => ToString_canonicalImpl();
    }
}
