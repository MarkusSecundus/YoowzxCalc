using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public abstract class DSLVisitorBaseNoRetVal<TRet, TContext> : IDSLVisitor<TRet, TContext>
    {
        public virtual void Visit(DSLFunctionDefinition expr, TContext ctx) => throw new NotImplementedException();


        public virtual void Visit(DSLConstantExpression expr, TContext ctx) => Visit((DSLPrimaryExpression)expr, ctx);

        public virtual void Visit(DSLArgumentExpression expr, TContext ctx) => Visit((DSLPrimaryExpression)expr, ctx);


        public virtual void Visit(DSLUnaryMinusExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);

        public virtual void Visit(DSLUnaryPlusExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);


        public virtual void Visit(DSLAddExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLSubtractExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLMultiplyExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLDivideExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLModuloExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLExponentialExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual void Visit(DSLFunctioncallExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);





        public virtual void Visit(DSLUnaryLogicalNotExpression expr, TContext ctx) => Visit((DSLUnaryExpression)expr, ctx);


        public virtual void Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLCompareGreaterThanExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLCompareLessOrEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLCompareLessThanExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);


        public virtual void Visit(DSLCompareIsEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLCompareIsNotEqualExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual void Visit(DSLLogicalAndExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);

        public virtual void Visit(DSLLogicalOrExpression expr, TContext ctx) => Visit((DSLBinaryExpression)expr, ctx);



        public virtual void Visit(DSLConditionalExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);











        public virtual void Visit(DSLPrimaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);

        public virtual void Visit(DSLUnaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);


        public virtual void Visit(DSLBinaryExpression expr, TContext ctx) => Visit((DSLExpression)expr, ctx);


        public virtual void Visit(DSLExpression expr, TContext ctx) => throw new NotImplementedException();


        TRet IDSLVisitor<TRet, TContext>.Visit(DSLFunctionDefinition expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLConstantExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLArgumentExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryMinusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryPlusExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLAddExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLSubtractExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLMultiplyExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLDivideExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLModuloExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLExponentialExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLFunctioncallExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLUnaryLogicalNotExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareGreaterThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareLessOrEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareLessThanExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareIsEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLCompareIsNotEqualExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLLogicalAndExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLLogicalOrExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
        TRet IDSLVisitor<TRet, TContext>.Visit(DSLConditionalExpression expr, TContext ctx) { Visit(expr, ctx); return default; }
    }
}
