﻿namespace MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression
{
    public sealed class DSLConstantExpression : DSLPrimaryExpression
    {
        public override T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public string Value { get; init; }


        public override string ToString() => $"|{Value}|";

        protected override bool Equals_impl(object obj) => obj is DSLConstantExpression e && Value == e.Value;

        protected override int ComputeHashCode() => Value.GetHashCode();
    }
}
