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
    class ASTValidator<TNumber> : DSLVisitorBaseNoReturnNoContext<object, object> where TNumber : INumber<TNumber>
    {
        private readonly IConstantParser<TNumber> _parser;

        public readonly List<Exception> IssuesFound = new();


        public ASTValidator(IConstantParser<TNumber> parser) => _parser = parser;



        public override void Visit(DSLFunctionDefinition expr)
        {
            var argumentsSet = expr.Arguments.ToHashSet();
            if(argumentsSet.Count != expr.Arguments.Count)
            {
                IssuesFound.Add(new Exception($"Duplicit argument names found in {DSLFunctionDefinition.HeadRepr(expr)}"));
            }
        }

        public override void Visit(DSLConstantExpression expr)
        {
            try
            {
                _parser.Parse(expr.Value);
            }
            catch(Exception e)
            {
                IssuesFound.Add(e);
            }
        }


        public override void Visit(DSLExpression expr)
        {
            foreach (var e in expr)
                e.Accept(this);
        }
    }
}
