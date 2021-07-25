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
    public abstract class DSLVisitorBase<T> : IDSLVisitor<T>
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



        public virtual T Visit(DSLTernaryExpression expr) => Visit((DSLExpression)expr);











        public virtual T Visit(DSLPrimaryExpression expr) => Visit((DSLExpression)expr);

        public virtual T Visit(DSLUnaryExpression expr) => Visit((DSLExpression)expr);


        public virtual T Visit(DSLBinaryExpression expr) => Visit((DSLExpression)expr);


        public virtual T Visit(DSLExpression expr) => throw new NotImplementedException();





    }
}
