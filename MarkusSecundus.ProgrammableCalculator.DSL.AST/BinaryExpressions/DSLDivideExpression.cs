using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions
{
    public sealed class DSLDivideExpression : DSLBinaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Accept(this);

        public override string Symbol => "/";
    }
}
