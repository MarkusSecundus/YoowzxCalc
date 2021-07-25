using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions
{
    public sealed class DSLCompareGreaterOrEqualExpression : DSLBinaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);

        public override string Symbol => ">=";
    }
}
