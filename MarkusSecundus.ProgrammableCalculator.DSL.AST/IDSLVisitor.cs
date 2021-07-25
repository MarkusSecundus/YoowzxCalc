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
        public T Visit(DSLFunctionDefinition expr);

        public T Visit(DSLConstantExpression expr);
        public T Visit(DSLArgumentExpression expr);

        public T Visit(DSLUnaryMinusExpression expr);
        public T Visit(DSLUnaryPlusExpression expr);

        public T Visit(DSLAddExpression expr);
        public T Visit(DSLSubtractExpression expr);
        public T Visit(DSLMultiplyExpression expr);
        public T Visit(DSLDivideExpression expr);
        public T Visit(DSLModuloExpression expr);
        public T Visit(DSLExponentialExpression expr);

        public T Visit(DSLFunctioncallExpression expr);
    }
}
