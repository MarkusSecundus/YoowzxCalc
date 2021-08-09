using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;

namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public interface IYCVisitor<out TRet, in TContext>
    {
        public TRet Visit(YCConstantExpression expr, TContext ctx);
        public TRet Visit(YCArgumentExpression expr, TContext ctx);

        public TRet Visit(YCUnaryMinusExpression expr, TContext ctx);
        public TRet Visit(YCUnaryPlusExpression expr, TContext ctx);

        public TRet Visit(YCAddExpression expr, TContext ctx);
        public TRet Visit(YCSubtractExpression expr, TContext ctx);
        public TRet Visit(YCMultiplyExpression expr, TContext ctx);
        public TRet Visit(YCDivideExpression expr, TContext ctx);
        public TRet Visit(YCModuloExpression expr, TContext ctx);
        public TRet Visit(YCExponentialExpression expr, TContext ctx);


        public TRet Visit(YCFunctioncallExpression expr, TContext ctx);



        public TRet Visit(YCUnaryLogicalNotExpression expr, TContext ctx);

        public TRet Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx);
        public TRet Visit(YCCompareGreaterThanExpression expr, TContext ctx);
        public TRet Visit(YCCompareLessOrEqualExpression expr, TContext ctx);
        public TRet Visit(YCCompareLessThanExpression expr, TContext ctx);

        public TRet Visit(YCCompareIsEqualExpression expr, TContext ctx);
        public TRet Visit(YCCompareIsNotEqualExpression expr, TContext ctx);


        public TRet Visit(YCLogicalAndExpression expr, TContext ctx);
        public TRet Visit(YCLogicalOrExpression expr, TContext ctx);

        public TRet Visit(YCConditionalExpression expr, TContext ctx);

    }
}
