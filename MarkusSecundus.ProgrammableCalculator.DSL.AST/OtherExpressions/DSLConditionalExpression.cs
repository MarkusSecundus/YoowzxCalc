using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions
{
    public sealed class DSLConditionalExpression : DSLExpression
    {
        public override T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public DSLExpression Condition { get; init; }
        public DSLExpression IfTrue { get; init; }
        public DSLExpression IfFalse { get; init; }


        public override DSLExpression this[int childIndex] => new[] { Condition, IfTrue, IfFalse}[childIndex];

        public override int Arity => 3;


        public override string ToString() => $"({Condition} ? {IfTrue} : {IfFalse})";

        protected override bool Equals_impl(object obj) => obj is DSLConditionalExpression b && Equals(Condition, b.Condition) && Equals(IfTrue, b.IfTrue) && Equals(IfFalse, b.IfFalse);

        protected override int ComputeHashCode() => (Condition, IfTrue, IfFalse).GetHashCode();
    }
}
