using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;


namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public static class YCVisitorExtensions
    {
        public static void Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoReturn<TRet, TContext> visitor, TContext ctx)
            => self.Accept(visitor, ctx);

        public static TRet Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);

        public static void Accept<TRet, TContext>(this YCExpression self, YCVisitorBaseNoReturnNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);
    }
}
