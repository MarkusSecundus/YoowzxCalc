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
    public abstract class DSLVisitorBaseNoContext<T, TContext> : IDSLVisitor<T, TContext>
    {
        public virtual T Visit(DSLFunctionDefinition expr) => throw new NotImplementedException();


        public virtual T Visit(DSLConstantExpression expr) => Visit((DSLPrimaryExpression)expr);

        public virtual T Visit(DSLArgumentExpression expr) => Visit((DSLPrimaryExpression)expr);


        public virtual T Visit(DSLUnaryMinusExpression expr) => Visit((DSLUnaryExpression)expr);

        public virtual T Visit(DSLUnaryPlusExpression expr) => Visit((DSLUnaryExpression)expr);


        public virtual T Visit(DSLAddExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLSubtractExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLMultiplyExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLDivideExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLModuloExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLExponentialExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual T Visit(DSLFunctioncallExpression expr) => Visit((DSLExpression)expr);





        public virtual T Visit(DSLUnaryLogicalNotExpression expr) => Visit((DSLUnaryExpression)expr);


        public virtual T Visit(DSLCompareGreaterOrEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLCompareGreaterThanExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLCompareLessOrEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLCompareLessThanExpression expr) => Visit((DSLBinaryExpression)expr);


        public virtual T Visit(DSLCompareIsEqualExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLCompareIsNotEqualExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual T Visit(DSLLogicalAndExpression expr) => Visit((DSLBinaryExpression)expr);

        public virtual T Visit(DSLLogicalOrExpression expr) => Visit((DSLBinaryExpression)expr);



        public virtual T Visit(DSLConditionalExpression expr) => Visit((DSLExpression)expr);











        public virtual T Visit(DSLPrimaryExpression expr) => Visit((DSLExpression)expr);

        public virtual T Visit(DSLUnaryExpression expr) => Visit((DSLExpression)expr);


        public virtual T Visit(DSLBinaryExpression expr) => Visit((DSLExpression)expr);


        public virtual T Visit(DSLExpression expr) => throw new NotImplementedException();




        T IDSLVisitor<T, TContext>.Visit(DSLConstantExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLArgumentExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLUnaryMinusExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLUnaryPlusExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLAddExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLSubtractExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLMultiplyExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLDivideExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLModuloExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLExponentialExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLFunctioncallExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLUnaryLogicalNotExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareGreaterThanExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareLessOrEqualExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareLessThanExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareIsEqualExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLCompareIsNotEqualExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLLogicalAndExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLLogicalOrExpression expr, TContext ctx) => Visit(expr);
        T IDSLVisitor<T, TContext>.Visit(DSLConditionalExpression expr, TContext ctx) => Visit(expr);
    }
}
