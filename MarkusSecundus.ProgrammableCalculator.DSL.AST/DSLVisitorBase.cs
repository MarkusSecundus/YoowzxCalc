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
        public T Accept(DSLConstantExpression expr) => Accept((DSLPrimaryExpression)expr);

        public T Accept(DSLArgumentExpression expr) => Accept((DSLPrimaryExpression)expr);


        public T Accept(DSLUnaryMinusExpression expr) => Accept((DSLUnaryExpression)expr);

        public T Accept(DSLUnaryPlusExpression expr) => Accept((DSLUnaryExpression)expr);


        public T Accept(DSLAddExpression expr) => Accept((DSLBinaryExpression)expr);

        public T Accept(DSLSubtractExpression expr) => Accept((DSLBinaryExpression)expr);

        public T Accept(DSLMultiplyExpression expr) => Accept((DSLBinaryExpression)expr);

        public T Accept(DSLDivideExpression expr) => Accept((DSLBinaryExpression)expr);

        public T Accept(DSLModuloExpression expr) => Accept((DSLBinaryExpression)expr);

        public T Accept(DSLExponentialExpression expr) => Accept((DSLBinaryExpression)expr);



        public T Accept(DSLFunctioncallExpression expr) => Accept((DSLExpression)expr);


        public T Accept(DSLPrimaryExpression expr) => Accept((DSLExpression)expr);

        public T Accept(DSLUnaryExpression expr) => Accept((DSLExpression)expr);


        public T Accept(DSLBinaryExpression expr) => Accept((DSLExpression)expr);


        public T Accept(DSLExpression expr) => throw new NotImplementedException();
    }
}
