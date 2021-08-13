using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCVisitorBaseNoReturn<TContext> : YCVisitorBaseNoReturn<object, TContext> { }

    public abstract class YCVisitorBaseNoReturn<TRet, TContext> : IYCVisitor<TRet, TContext>
    {
        public virtual void Visit(YCLiteralExpression expr, TContext ctx) => Visit((YCPrimaryExpression)expr, ctx);


        public virtual void Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);

        public virtual void Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual void Visit(YCAddExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCSubtractExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCMultiplyExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCDivideExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCModuloExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCExponentialExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual void Visit(YCFunctioncallExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);





        public virtual void Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit((YCUnaryExpression)expr, ctx);


        public virtual void Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);


        public virtual void Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual void Visit(YCLogicalAndExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);

        public virtual void Visit(YCLogicalOrExpression expr, TContext ctx) => Visit((YCBinaryExpression)expr, ctx);



        public virtual void Visit(YCConditionalExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);











        public virtual void Visit(YCPrimaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);

        public virtual void Visit(YCUnaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual void Visit(YCBinaryExpression expr, TContext ctx) => Visit((YCExpression)expr, ctx);


        public virtual void Visit(YCExpression expr, TContext ctx) => throw new NotImplementedException();


        TRet IYCVisitor<TRet, TContext>.Visit(YCLiteralExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryMinusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryPlusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCAddExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCSubtractExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCMultiplyExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCDivideExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCModuloExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCExponentialExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCFunctioncallExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCUnaryLogicalNotExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareGreaterThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareLessThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCCompareIsNotEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalAndExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCLogicalOrExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IYCVisitor<TRet, TContext>.Visit(YCConditionalExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
    }
}
