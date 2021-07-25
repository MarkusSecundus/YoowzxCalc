using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions
{
    public sealed class DSLFunctioncallExpression : DSLExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);

        public string Name { get; init; }

        public IReadOnlyList<DSLExpression> Arguments { get; init; }

        public override DSLExpression this[int childIndex] => Arguments[childIndex];

        public override int Arity => Arguments.Count;


        public override string ToString() => $"{Name}({Arguments.Concat()})";
    }
}
