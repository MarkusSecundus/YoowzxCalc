using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression
{
    public sealed class DSLArgumentExpression : DSLPrimaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Accept(this);

        public string ArgumentName { get; init; }
    }
}
