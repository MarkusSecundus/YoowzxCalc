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
    public class ASTValidator<TNumber> : DSLVisitorBase<object> where TNumber : INumber<TNumber>
    {
        private readonly IConstantParser<TNumber> _parser;

        public readonly List<Exception> IssuesFound = new();


        public ASTValidator(IConstantParser<TNumber> parser) => _parser = parser;



        public override object Visit(DSLFunctionDefinition expr)
        {
            var argumentsSet = expr.Arguments.ToHashSet();
            if(argumentsSet.Count != expr.Arguments.Count)
            {
                IssuesFound.Add(new Exception($"Duplicit argument names found in {DSLFunctionDefinition.HeadRepr(expr)}"));
            }
            return null;
        }

        public override object Visit(DSLConstantExpression expr)
        {
            try
            {
                _parser.Parse(expr.Value);
            }
            catch(Exception e)
            {
                IssuesFound.Add(e);
            }
            return null;
        }


        public override object Visit(DSLExpression expr) => null;
    }
}
