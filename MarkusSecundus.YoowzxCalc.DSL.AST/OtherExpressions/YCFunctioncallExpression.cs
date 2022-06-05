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
        /// <inheritdoc/>
        public override T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);

        /// <summary>
        /// Arbitrary string representing the name of the function to be called.
        /// Asserting its validity is left up to the user to allow more flexibility.
        /// </summary>
        public string Name { get; init; }


        /// <summary>
        /// List of argument subexpressions in order from left to right.
        /// </summary>
        public IReadOnlyList<YCExpression> Arguments 
        { 
            get => _arguments; 
            init => _arguments = value is ListComparedByContents<YCExpression> e
                ? e
                : new ListComparedByContents<YCExpression>(value);
        }
        private ListComparedByContents<YCExpression> _arguments = EmptyArguments;


        /// <inheritdoc/>
        public override YCExpression this[int childIndex] => Arguments[childIndex];

        /// <inheritdoc/>
        public override int Arity => Arguments.Count;


        /// <inheritdoc/>
        public override string ToString() => $"{Name}({Arguments.MakeString()})";

        /// <inheritdoc/>
        protected override int ComputeHashCode() => (Name, Arguments.SequenceHashCode()).GetHashCode();



        private static readonly ListComparedByContents<YCExpression> EmptyArguments = new ListComparedByContents<YCExpression>(CollectionsUtils.EmptyList<YCExpression>());
    }
}
