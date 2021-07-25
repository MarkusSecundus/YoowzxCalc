﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLBinaryExpression : DSLExpression
    {
        internal DSLBinaryExpression() { }


        public DSLExpression LeftChild { get; init; }
        public DSLExpression RightChild { get; init; }

        public sealed override int Arity => 2;

        public sealed override DSLExpression this[int childIndex] 
            => childIndex == 0 ? LeftChild : 
               childIndex == 1 ? RightChild :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");

        public abstract string Symbol { get; }




        public override string ToString() => $"({LeftChild} {Symbol} {RightChild})";

        protected override bool Equals_impl(object obj) => obj is DSLBinaryExpression b && GetType() == b.GetType() && Equals(LeftChild, b.LeftChild) && Equals(RightChild, b.RightChild);

        protected override int ComputeHashCode() => (LeftChild, RightChild).GetHashCode();
    }
}
