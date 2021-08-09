using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCPrimaryExpression : YCExpression
    {
        internal YCPrimaryExpression() { }


        public sealed override int Arity => 0;

        public sealed override YCExpression this[int childIndex]
            => throw new IndexOutOfRangeException($"Is a primary expression and therefore has 0 children");
    }
}
