using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCVisitorBaseNoReturnNoContext<TRet, TContext> : IYCVisitor<TRet, TContext>
    {
        public virtual void Visit(YCConstantExpression expr) => Visit((YCPrimaryExpression)expr);

        public virtual void Visit(YCArgumentExpression expr) => Visit((YCPrimaryExpression)expr);


        public virtual void Visit(YCUnaryMinusExpression expr) => Visit((YCUnaryExpression)expr);

        public virtual void Visit(YCUnaryPlusExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual void Visit(YCAddExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCSubtractExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCMultiplyExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCDivideExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCModuloExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCExponentialExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual void Visit(YCFunctioncallExpression expr) => Visit((YCExpression)expr);





        public virtual void Visit(YCUnaryLogicalNotExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual void Visit(YCCompareGreaterOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCCompareGreaterThanExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCCompareLessOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCCompareLessThanExpression expr) => Visit((YCBinaryExpression)expr);


        public virtual void Visit(YCCompareIsEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCCompareIsNotEqualExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual void Visit(YCLogicalAndExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual void Visit(YCLogicalOrExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual void Visit(YCConditionalExpression expr) => Visit((YCExpression)expr);











        public virtual void Visit(YCPrimaryExpression expr) => Visit((YCExpression)expr);

        public virtual void Visit(YCUnaryExpression expr) => Visit((YCExpression)expr);


        public virtual void Visit(YCBinaryExpression expr) => Visit((YCExpression)expr);


        public virtual void Visit(YCExpression expr) => throw new NotImplementedException();



        TRet IYCVisitor<TRet, TContext>.Visit(YCConstantExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCArgumentExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryMinusExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryPlusExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCAddExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCSubtractExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCMultiplyExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCDivideExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCModuloExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCExponentialExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCFunctioncallExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryLogicalNotExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterThanExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessOrEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessThanExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsNotEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalAndExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalOrExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCConditionalExpression expr, TContext ctx) { Visit(expr); return default; }
    }
}
