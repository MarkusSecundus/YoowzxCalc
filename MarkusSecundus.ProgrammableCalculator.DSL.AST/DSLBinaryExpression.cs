using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLBinaryExpression : DSLExpression
    {
        public DSLExpression LeftChild { get; init; }
        public DSLExpression RightChild { get; init; }

        public sealed override int Arity => 2;

        public sealed override DSLExpression this[int childIndex] 
            => childIndex == 0 ? LeftChild : 
               childIndex == 1 ? RightChild :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");
    }
}
