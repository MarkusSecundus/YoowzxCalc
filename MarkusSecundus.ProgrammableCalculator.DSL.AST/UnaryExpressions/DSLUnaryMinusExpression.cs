using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions
{
    public sealed class DSLUnaryMinusExpression : DSLUnaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Accept(this);
    }
}
