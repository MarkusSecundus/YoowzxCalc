using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLExpression
    {
        internal DSLExpression() { }

        public abstract T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx);

        public abstract int Arity { get; }
        public abstract DSLExpression this[int childIndex] { get; }








        private int? _hashCode;

        public override int GetHashCode() => _hashCode ??= ComputeHashCode();

        public sealed override bool Equals(object obj) => Equals_impl(obj);

        protected abstract bool Equals_impl(object o);
        protected abstract int ComputeHashCode();
    }
}
