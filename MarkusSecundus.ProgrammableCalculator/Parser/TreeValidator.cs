using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public class TreeValidator<TNumber> : DSLVisitorBase<object> where TNumber : INumber<TNumber>
    {
        private readonly IConstantParser<TNumber> _parser;



        public TreeValidator(IConstantParser<TNumber> parser)
        {
            _parser = parser;
        }

        public override object Visit(DSLFunctionDefinition expr)
        {

            return null;
        }

        public override object Visit(DSLConstantExpression expr)
        {

            return null;
        }

        public override object Visit(DSLExpression expr) => null;
    }
}
