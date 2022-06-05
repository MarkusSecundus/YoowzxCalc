using System;
using System.Collections.Generic;
using static MarkusSecundus.Util.CollectionsUtils;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;


namespace MarkusSecundus.YoowzxCalc.DSL.AST
{


    /// <summary>
    /// Abstract base for any subtree in abstract syntax tree of a YoowzxCalc expression.
    /// 
    /// Is immutable and supports visitor pattern implemented by <see cref="IYCVisitor{TRet, TContext}"/>
    /// </summary>
    public abstract record YCExpression : IReadOnlyList_PreimplementedEnumerator<YCExpression>
    {
        internal YCExpression() { }

        /// <summary>
        /// Accept a visitor.
        /// </summary>
        /// <typeparam name="TRet">Result type of the visit.</typeparam>
        /// <typeparam name="TContext">Context type of the visit.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="ctx">Context for the visit.</param>
        /// <returns>Result of the visit.</returns>
        public abstract TRet Accept<TRet, TContext>(IYCVisitor<TRet, TContext> visitor, TContext ctx);

        /// <summary>
        /// Number of children this node has.
        /// </summary>
        public abstract int Arity { get; }

        int IReadOnlyCollection<YCExpression>.Count => Arity;

        /// <summary>
        /// Gets n-th child of this expression node.
        /// </summary>
        /// <param name="childIndex">Index of the child to get.</param>
        /// <exception cref="IndexOutOfRangeException">When childIndex is negative or not less-than Arity</exception>
        /// <returns>childIndex-th child of this</returns>
        public abstract YCExpression this[int childIndex] { get; }


        //private int? _hashCode;

        /// <inheritdoc/>
        public override int GetHashCode() => ComputeHashCode();//_hashCode ??= ComputeHashCode();
        /// <summary>
        /// Perform computation of this's hashcode. 
        /// <para/>
        /// Gets called only once and than is cached, thus may be arbitrarily slow.
        /// </summary>
        /// <returns>Hash code of this</returns>
        protected abstract int ComputeHashCode();
    }
}
