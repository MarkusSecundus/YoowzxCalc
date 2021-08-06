using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public interface IDSLVisitor<out TRet, in TContext>
    {
        public TRet Visit(DSLConstantExpression expr, TContext ctx);
        public TRet Visit(DSLArgumentExpression expr, TContext ctx);

        public TRet Visit(DSLUnaryMinusExpression expr, TContext ctx);
        public TRet Visit(DSLUnaryPlusExpression expr, TContext ctx);

        public TRet Visit(DSLAddExpression expr, TContext ctx);
        public TRet Visit(DSLSubtractExpression expr, TContext ctx);
        public TRet Visit(DSLMultiplyExpression expr, TContext ctx);
        public TRet Visit(DSLDivideExpression expr, TContext ctx);
        public TRet Visit(DSLModuloExpression expr, TContext ctx);
        public TRet Visit(DSLExponentialExpression expr, TContext ctx);
        

        public TRet Visit(DSLFunctioncallExpression expr, TContext ctx);



        public TRet Visit(DSLUnaryLogicalNotExpression expr, TContext ctx);

        public TRet Visit(DSLCompareGreaterOrEqualExpression expr, TContext ctx);
        public TRet Visit(DSLCompareGreaterThanExpression expr, TContext ctx);
        public TRet Visit(DSLCompareLessOrEqualExpression expr, TContext ctx);
        public TRet Visit(DSLCompareLessThanExpression expr, TContext ctx);

        public TRet Visit(DSLCompareIsEqualExpression expr, TContext ctx);
        public TRet Visit(DSLCompareIsNotEqualExpression expr, TContext ctx);


        public TRet Visit(DSLLogicalAndExpression expr, TContext ctx);
        public TRet Visit(DSLLogicalOrExpression expr, TContext ctx);

        public TRet Visit(DSLConditionalExpression expr, TContext ctx);

    }
}
