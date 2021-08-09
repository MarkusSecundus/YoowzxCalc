using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;


namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public static class DSLVisitorExtensions
    {
        public static void Accept<TRet, TContext>(this DSLExpression self, DSLVisitorBaseNoReturn<TRet, TContext> visitor, TContext ctx)
            => self.Accept(visitor, ctx);

        public static TRet Accept<TRet, TContext>(this DSLExpression self, DSLVisitorBaseNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);

        public static void Accept<TRet, TContext>(this DSLExpression self, DSLVisitorBaseNoReturnNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);
    }
}
