using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl
{
    struct YCIdentifierValidatorArgs<TNumber>
    {
        public IYCNumberOperator<TNumber> Op { get; init; }
        public List<FormatException> Exceptions { get; init; }
    }

    class YCIdentifierValidator<TNumber> : YCVisitorBaseNoReturn<YCIdentifierValidatorArgs<TNumber>>
    {
        protected YCIdentifierValidator() { }
        public static YCIdentifierValidator<TNumber> Instance { get; } = new();


        private void checkIdentifierValidity(string expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            var error = ctx.Op.ValidateIdentifier(expr);
            if (error != null) ctx.Exceptions.Add(error);
        }

        private void checkArgumentDuplicity(IReadOnlyCollection<string> exprs, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            if (exprs.CheckHasDuplicit() )
                ctx.Exceptions.Add(new FormatException($"Some arguments are duplicit: [{exprs.MakeString()}]"));
        }

        public List<FormatException> Scan(YCFunctionDefinition def, IYCNumberOperator<TNumber> op)
        {
            YCIdentifierValidatorArgs<TNumber> ctx = new() { Op = op, Exceptions = new() };

            if(!def.IsAnonymous)
                checkIdentifierValidity(def.Name, ctx);
            foreach (var arg in def.Arguments)
                checkIdentifierValidity(arg, ctx);

            checkArgumentDuplicity(def.Arguments, ctx);

            def.Body.Accept(this, ctx);

            return ctx.Exceptions;
        }

        public void Validate(YCFunctionDefinition def, IYCNumberOperator<TNumber> op)
        {
            var errors = Scan(def, op);
            if (errors != null && !errors.IsEmpty())
                throw errors.Aggregate<FormatException>();
        }


        public override void Visit(YCLiteralExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            if (!ctx.Op.TryParseConstant(expr.Value, out _))
            {
                checkIdentifierValidity(expr.Value, ctx);
            }
        }

        public override void Visit(YCFunctioncallExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            checkIdentifierValidity(expr.Name, ctx);
            Visit((YCExpression)expr, ctx);
        }

        public override void Visit(YCExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            foreach (var subexpr in expr)
                subexpr.Accept(this, ctx);
        }
    }
}
