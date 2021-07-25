using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public class ASTParser<TNumber> : DSLVisitorBase<Expression> where TNumber : INumber<TNumber>
    {
        private readonly IConstantParser<TNumber> _constantParser;

        public ASTParser(IConstantParser<TNumber> constantParser) => _constantParser = constantParser;


        public override Expression Visit(DSLConstantExpression expr)
        {
            return Expression.Constant(_constantParser.Parse(expr.Value));
        }


    }
}
