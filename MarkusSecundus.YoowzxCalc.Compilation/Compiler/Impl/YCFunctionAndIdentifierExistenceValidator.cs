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
    /// <summary>
    /// Compiler decorator that takes care of simple rigid compile-time validation of whether all identifiers used in the expression are defined.
    /// </summary>
    /// <typeparam name="TNumber">Number type being operated on</typeparam>
    [Obsolete("Experimental preview feature - subject to change")]
    public class YCFunctionAndIdentifierExistenceValidatedCompiler<TNumber> : IYCCompiler<TNumber>
    {
        /// <summary>
        /// Name of the annotation used to switch this validator on/of.
        /// </summary>
        public const string SwitchAnnotation = "validate_identifiers";

        /// <summary>
        /// Inner compiler implementation
        /// </summary>
        public IYCCompiler<TNumber> Base { get; }

        /// <summary>
        /// If <c>false</c>, the identifier checking will always be performed.
        /// If <c>true</c>, the compiler will first check whether the `validate_identifiers` annotation is present and use its value as bool to determine whether the validation shall be done.
        /// E.g. [validate_identifiers: true] f(x) := ...
        /// E.g. [validate_identifiers: false] f(x) := ...
        /// </summary>
        public bool IsAnnotationSwitchable { get; init; } = true;

        /// <summary>
        /// Default value for being switched on/off used if not overriden by the `validate_identifiers` annotation
        /// </summary>
        public bool DefaultTurnedOnOffState { get; init; } = false;

        /// <summary>
        /// Constructs the instance using supplied base compiler
        /// </summary>
        /// <param name="baseCompiler">Inner compiler implementation</param>
        public YCFunctionAndIdentifierExistenceValidatedCompiler(IYCCompiler<TNumber> baseCompiler) => Base = baseCompiler;

        /// <inheritdoc/>
        public IYCNumberOperator<TNumber> NumberOperator => Base.NumberOperator;

        /// <inheritdoc/>
        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile)
        {
            if (ShouldDoValidation(toCompile))
            {
                var exceptions = toCompile.Body.Accept(Visitor.Instance, CreateContext(ctx, toCompile)).ToList();
                if (exceptions.Count > 0)
                    throw new FormatException("Undef", new AggregateException(exceptions));
            }

            return Base.Compile(ctx, toCompile);
        }

        private bool ShouldDoValidation(YCFunctionDefinition def)
        {
            if (!IsAnnotationSwitchable) return true;
            if (!def.Annotations.TryGetValue(SwitchAnnotation, out var stringRepr) || !bool.TryParse(stringRepr, out var result))
                return DefaultTurnedOnOffState;
            return result;
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

                foreach (var e in Visit((YCExpression)expr, ctx))
                    yield return e;
            }
        }
    }
}
