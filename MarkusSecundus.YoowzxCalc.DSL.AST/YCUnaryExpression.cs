using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCUnaryExpression : YCExpression
    {
        internal YCUnaryExpression() { }


        public YCExpression Child { get; init; }

        public sealed override int Arity => 1;

        public sealed override YCExpression this[int childIndex]
            => childIndex == 0 ? Child :
                throw new IndexOutOfRangeException($"Index {childIndex} not in range <0;{Arity})");

        public abstract string Symbol { get; }

        public override string ToString() => $"({Symbol}{Child})";

        protected override bool Equals_impl(object obj) => obj is YCUnaryExpression e && GetType() == e.GetType() && Equals(Child, e.Child);

        protected override int ComputeHashCode() => Child.GetHashCode();
    }
}
