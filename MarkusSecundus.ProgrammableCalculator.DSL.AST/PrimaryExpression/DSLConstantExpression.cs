using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression
{
    public sealed class DSLConstantExpression : DSLPrimaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);

        public string Value { get; init; }


        public override string ToString() => Value;

        protected override bool Equals_impl(object obj) => obj is DSLConstantExpression e && Value == e.Value;

        protected override int ComputeHashCode() => Value.GetHashCode();
    }
}
