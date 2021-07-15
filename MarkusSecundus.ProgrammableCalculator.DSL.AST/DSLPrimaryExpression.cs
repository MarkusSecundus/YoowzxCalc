using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLPrimaryExpression : DSLExpression
    {
        public sealed override int Arity => 0;

        public sealed override DSLExpression this[int childIndex]
            => throw new IndexOutOfRangeException($"Is a primary expression and therefore has 0 children");
    }
}
