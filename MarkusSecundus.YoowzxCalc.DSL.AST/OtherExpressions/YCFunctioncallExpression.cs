using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions
{
    public sealed record YCFunctioncallExpression : YCExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        public string Name { get; init; }

        public IReadOnlyList<YCExpression> Arguments { get; init; }

        public override YCExpression this[int childIndex] => Arguments[childIndex];

        public override int Arity => Arguments.Count;


        public override string ToString() => $"{Name}({Arguments.MakeString()})";

        protected override int ComputeHashCode() => (Name, Arguments.SequenceHashCode()).GetHashCode();
    }
}
