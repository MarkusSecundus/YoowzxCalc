using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    class ASTRecursionDetector : DSLVisitorBase<bool, (string Name, bool ShouldCheckVariables)>
    {
        private ASTRecursionDetector() { }


        public static ASTRecursionDetector Instance { get; } = new();

        public override bool Visit(DSLFunctionDefinition expr, (string Name, bool ShouldCheckVariables) ctx)
        {
            bool shouldCheckVariables = expr.Arguments.Count == 0 && !expr.Arguments.Contains(expr.Name);
            return expr.Body.Accept(this, (expr.Name, shouldCheckVariables));
        }

        public override bool Visit(DSLFunctioncallExpression expr, (string Name, bool ShouldCheckVariables) ctx)
        {
            return expr.Name == ctx.Name || Visit((DSLExpression)expr, ctx);
        }

        public override bool Visit(DSLArgumentExpression expr, (string Name, bool ShouldCheckVariables) ctx)
        {
            return ctx.ShouldCheckVariables && expr.ArgumentName == ctx.Name;
        }

        public override bool Visit(DSLExpression expr, (string Name, bool ShouldCheckVariables) ctx)
        {
            foreach (var subexpr in expr)
                if (subexpr.Accept(this, ctx))
                    return true;
            return false;
        }
    }
}
