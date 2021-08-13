using System;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public abstract class YCVisitorBaseNoContext<T> : YCVisitorBaseNoContext<T, object> { }

    public abstract class YCVisitorBaseNoContext<T, TContext> : IYCVisitor<T, TContext>
    {
        public virtual T Visit(YCLiteralExpression expr) => Visit((YCPrimaryExpression)expr);


        public virtual T Visit(YCUnaryMinusExpression expr) => Visit((YCUnaryExpression)expr);

        public virtual T Visit(YCUnaryPlusExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual T Visit(YCAddExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCSubtractExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCMultiplyExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCDivideExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCModuloExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCExponentialExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual T Visit(YCFunctioncallExpression expr) => Visit((YCExpression)expr);





        public virtual T Visit(YCUnaryLogicalNotExpression expr) => Visit((YCUnaryExpression)expr);


        public virtual T Visit(YCCompareGreaterOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCCompareGreaterThanExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCCompareLessOrEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCCompareLessThanExpression expr) => Visit((YCBinaryExpression)expr);


        public virtual T Visit(YCCompareIsEqualExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCCompareIsNotEqualExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual T Visit(YCLogicalAndExpression expr) => Visit((YCBinaryExpression)expr);

        public virtual T Visit(YCLogicalOrExpression expr) => Visit((YCBinaryExpression)expr);



        public virtual T Visit(YCConditionalExpression expr) => Visit((YCExpression)expr);











        public virtual T Visit(YCPrimaryExpression expr) => Visit((YCExpression)expr);

        public virtual T Visit(YCUnaryExpression expr) => Visit((YCExpression)expr);


        public virtual T Visit(YCBinaryExpression expr) => Visit((YCExpression)expr);


        public virtual T Visit(YCExpression expr) => throw new NotImplementedException();




        T IYCVisitor<T, TContext>.Visit(YCLiteralExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCUnaryMinusExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCUnaryPlusExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCAddExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCSubtractExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCMultiplyExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCDivideExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCModuloExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCExponentialExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCFunctioncallExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCUnaryLogicalNotExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareGreaterOrEqualExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareGreaterThanExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareLessOrEqualExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareLessThanExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareIsEqualExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCCompareIsNotEqualExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCLogicalAndExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCLogicalOrExpression expr, TContext ctx) => Visit(expr);
        T IYCVisitor<T, TContext>.Visit(YCConditionalExpression expr, TContext ctx) => Visit(expr);
    }
}
