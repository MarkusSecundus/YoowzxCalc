using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCBinaryExpression : YCExpression
    {
        internal YCBinaryExpression() { }


        public YCExpression LeftChild { get; init; }
        public YCExpression RightChild { get; init; }

        public sealed override int Arity => 2;

        public sealed override YCExpression this[int childIndex]
            => childIndex == 0 ? LeftChild :
               childIndex == 1 ? RightChild :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");

        public abstract string Symbol { get; }




        public override string ToString() => $"({LeftChild} {Symbol} {RightChild})";

        protected override bool Equals_impl(object obj) => obj is YCBinaryExpression b && GetType() == b.GetType() && Equals(LeftChild, b.LeftChild) && Equals(RightChild, b.RightChild);

        protected override int ComputeHashCode() => (LeftChild, RightChild).GetHashCode();
    }
}
