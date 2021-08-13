using MarkusSecundus.ProgrammableCalculator.Numerics;
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
        public INumberOperator<TNumber> Op { get; init; }
        public List<FormatException> Exceptions { get; init; }
    }

    class YCIdentifierValidator<TNumber> : YCVisitorBaseNoReturn<YCIdentifierValidatorArgs<TNumber>>
    {
        protected YCIdentifierValidator() { }
        public static YCIdentifierValidator<TNumber> Instance { get; } = new();


        private void validate(string expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            var error = ctx.Op.ValidateIdentifier(expr);
            if (error != null) ctx.Exceptions.Add(error);
        }

        private void validateDuplicity(IReadOnlyCollection<string> exprs, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            if (exprs.CheckHasDuplicit() )
                ctx.Exceptions.Add(new FormatException($"Some arguments are duplicit: [{exprs.MakeString()}]"));
        }

        public List<FormatException> Validate(YCFunctionDefinition def, INumberOperator<TNumber> op)
        {
            YCIdentifierValidatorArgs<TNumber> ctx = new() { Op = op, Exceptions = new() };

            if(!def.IsAnonymous)
                validate(def.Name, ctx);
            foreach (var arg in def.Arguments)
                validate(arg, ctx);

            validateDuplicity(def.Arguments, ctx);

            def.Body.Accept(this, ctx);

            return ctx.Exceptions;
        }


        public override void Visit(YCLiteralExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            if (!ctx.Op.TryParse(expr.Value, out _))
            {
                validate(expr.Value, ctx);
            }
        }

        public override void Visit(YCFunctioncallExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            validate(expr.Name, ctx);
            Visit((YCExpression)expr, ctx);
        }

        public override void Visit(YCExpression expr, YCIdentifierValidatorArgs<TNumber> ctx)
        {
            foreach (var subexpr in expr)
                subexpr.Accept(this, ctx);
        }
    }
}
