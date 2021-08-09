using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class DSLVisitorBaseNoReturnNoContext<TRet, TContext> : IDSLVisitor<TRet, TContext>
    {
        public virtual void Visit(DSLConstantExpression expr) => Visit((DSLPrimaryExpression)expr);

        public virtual void Visit(DSLArgumentExpression expr) => Visit((DSLPrimaryExpression)expr);


        public virtual void Visit(DSLUnaryMinusExpression expr) => Visit((DSLUnaryExpression)expr);

        public virtual void Visit(DSLUnaryPlusExpression expr) => Visit((DSLUnaryExpression)expr);


        public virtual void Visit(DSLAddExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLSubtractExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLMultiplyExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLDivideExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLModuloExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLExponentialExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual void Visit(DSLFunctioncallExpression expr) => Visit((DSLExpression)expr);





        public virtual void Visit(DSLUnaryLogicalNotExpression expr) => Visit((DSLUnaryExpression)expr);


        public virtual void Visit(DSLCompareGreaterOrEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLCompareGreaterThanExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLCompareLessOrEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLCompareLessThanExpression expr) => Visit((DSLBinaryExpression)expr);


        public virtual void Visit(DSLCompareIsEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLCompareIsNotEqualExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual void Visit(DSLLogicalAndExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual void Visit(DSLLogicalOrExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual void Visit(DSLConditionalExpression expr) => Visit((DSLExpression)expr);











        public virtual void Visit(DSLPrimaryExpression expr) => Visit((DSLExpression)expr);

        public virtual void Visit(DSLUnaryExpression expr) => Visit((DSLExpression)expr);


        public virtual void Visit(DSLBinaryExpression expr) => Visit((DSLExpression)expr);


        public virtual void Visit(DSLExpression expr) => throw new NotImplementedException();



        TRet IDSLVisitor<TRet, TContext>.Visit(DSLConstantExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLArgumentExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryMinusExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryPlusExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLAddExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLSubtractExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLMultiplyExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLDivideExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLModuloExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLExponentialExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLFunctioncallExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryLogicalNotExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareGreaterThanExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareLessOrEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareLessThanExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareIsEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareIsNotEqualExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLLogicalAndExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLLogicalOrExpression expr, TContext ctx) { Visit(expr); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLConditionalExpression expr, TContext ctx) { Visit(expr); return default; }
    }
}
