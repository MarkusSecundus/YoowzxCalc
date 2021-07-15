using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLUnaryExpression : DSLExpression
    {
        public DSLExpression Child { get; init; }

        public sealed override int Arity => 1;

        public sealed override DSLExpression this[int childIndex]
            => childIndex == 0 ? Child :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");
    }
}
