using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions
{
    public sealed class DSLTernaryExpression : DSLExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);

        public DSLExpression Condition { get; init; }
        public DSLExpression IfTrue { get; init; }
        public DSLExpression IfFalse { get; init; }


        public override DSLExpression this[int childIndex] => new[] { Condition, IfTrue, IfFalse}[childIndex];

        public override int Arity => 3;


        public override string ToString() => $"({Condition} ? {IfTrue} : {IfFalse})";
    }
}
