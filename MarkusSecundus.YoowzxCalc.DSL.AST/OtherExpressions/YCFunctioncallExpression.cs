using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions
{

    /// <summary>
    /// Node that represents calling a function.
    /// </summary>
    public sealed record YCFunctioncallExpression : YCExpression
    {
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        /// <summary>
        /// Arbitrary string representing the name of the function to be called.
        /// Asserting its validity is left up to the user to allow more flexibility.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// List of argument subexpressions in order from left to right.
        /// </summary>
        public IReadOnlyList<YCExpression> Arguments { get; init; }

        public override YCExpression this[int childIndex] => Arguments[childIndex];

        public override int Arity => Arguments.Count;


        public override string ToString() => $"{Name}({Arguments.MakeString()})";

        protected override int ComputeHashCode() => (Name, Arguments.SequenceHashCode()).GetHashCode();
    }
}
