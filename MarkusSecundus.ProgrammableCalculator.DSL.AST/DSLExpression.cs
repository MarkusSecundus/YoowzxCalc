using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLExpression
    {
        public abstract T Accept<T>(IDSLVisitor<T> visitor);

        public abstract int Arity { get; }
        public abstract DSLExpression this[int childIndex] { get; }
    }
}
