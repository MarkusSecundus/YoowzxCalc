﻿namespace MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions
{




    /// <summary>
    /// Node representing modulo expression (<c>`x % y`</c> in C-like languages)
    /// </summary>
    public sealed record YCModuloExpression : YCBinaryExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        internal override string Symbol => "%";

        public override string ToString() => ToString_canonicalImpl();
    }
}
