﻿using System;
using System.Collections.Generic;
using static MarkusSecundus.Util.CollectionsUtils;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




[assembly: CLSCompliant(true)]

namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCExpression : IReadOnlyList_PreimplementedEnumerator<YCExpression>
    {
        internal YCExpression() { }

        public abstract T Accept<T, TContext>(IYCVisitor<T, TContext> visitor, TContext ctx);

        public abstract int Arity { get; }

        int IReadOnlyCollection<YCExpression>.Count => Arity;

        public abstract YCExpression this[int childIndex] { get; }








        private int? _hashCode;

        public override int GetHashCode() => _hashCode ??= ComputeHashCode();

        public sealed override bool Equals(object obj) => Equals_impl(obj);

        protected abstract bool Equals_impl(object o);
        protected abstract int ComputeHashCode();

    }
}