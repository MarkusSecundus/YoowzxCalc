using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl
{
    class YCFunctionAndIdentifierExistenceValidatedCompiler<TNumber> : IYCCompiler<TNumber>
    {
        public IYCCompiler<TNumber> Base { get; }

        public YCFunctionAndIdentifierExistenceValidatedCompiler(IYCCompiler<TNumber> baseCompiler) => Base = baseCompiler;

        public IYCNumberOperator<TNumber> NumberOperator => Base.NumberOperator;

        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            var exceptions = toCompile.Body.Accept(Visitor.Instance, CreateContext(ctx, toCompile)).ToList();
            if (exceptions.Count > 0)
                throw new FormatException("", new AggregateException(exceptions));

            return Base.Compile(ctx, toCompile);
        }


        private VisitorContext CreateContext(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition def)
        {
            var ret = new VisitorContext { Signatures = new(), IsConstant = s=> NumberOperator.TryParseConstant(s, out _)};

            if (!def.IsAnonymous)
                ret.Signatures.Add(def.GetSignature<TNumber>());
            foreach (var functionArg in def.Arguments)
                ret.Signatures.Add(new YCFunctionSignature<TNumber>(functionArg, 0));

            foreach (var contextFunction in ctx.Functions.Keys)
                ret.Signatures.Add(contextFunction);
            foreach (var stdlibFunction in NumberOperator.StandardLibrary.Keys)
                ret.Signatures.Add(stdlibFunction);

            return ret;
        }

        struct VisitorContext
        {
            public HashSet<YCFunctionSignature<TNumber>> Signatures { get; init; }

            public Func<string, bool> IsConstant { get; init; }
        }

        class Visitor : YCVisitorBase<IEnumerable<Exception>, VisitorContext>
        {
            public static readonly Visitor Instance = new();

            public override IEnumerable<Exception> Visit(YCExpression expr, VisitorContext ctx)
            {
                foreach (var child in expr)
                    foreach(var e in child.Accept(this, ctx)) yield return e;
            }

            public override IEnumerable<Exception> Visit(YCLiteralExpression expr, VisitorContext ctx)
            {
                if (!ctx.IsConstant(expr.Value) && !ctx.Signatures.Contains(new YCFunctionSignature<TNumber>(expr.Value, 0)))
                    yield return new FormatException($"Undefined identifier: '{expr.Value}'");
            }

            public override IEnumerable<Exception> Visit(YCFunctioncallExpression expr, VisitorContext ctx)
            {
                var signature = expr.GetSignature<TNumber>();
                if (!ctx.Signatures.Contains(signature))
                    yield return new FormatException($"Undefined function: '{signature.ToStringTypeless()}'");
            }
        }
    }
}
