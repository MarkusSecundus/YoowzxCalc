using MarkusSecundus.ProgrammableCalculator.DSL.AST.BinaryExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.OtherExpressions;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.UnaryExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public interface IDSLVisitor<T>
    {
        public T Accept(DSLConstantExpression expr);
        public T Accept(DSLArgumentExpression expr);

        public T Accept(DSLUnaryMinusExpression expr);
        public T Accept(DSLUnaryPlusExpression expr);

        public T Accept(DSLAddExpression expr);
        public T Accept(DSLSubtractExpression expr);
        public T Accept(DSLMultiplyExpression expr);
        public T Accept(DSLDivideExpression expr);
        public T Accept(DSLModuloExpression expr);
        public T Accept(DSLExponentialExpression expr);

        public T Accept(DSLFunctioncallExpression expr);
    }
}
