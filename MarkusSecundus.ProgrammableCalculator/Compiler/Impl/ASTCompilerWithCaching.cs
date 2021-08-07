using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.Numerics;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTCompilerWithCaching<TNumber> : IASTCompiler<TNumber>
    {
        private readonly IASTCompiler<TNumber> _base;

        public ASTCompilerWithCaching(IASTCompiler<TNumber> baseCompiler)
            => _base = baseCompiler;

        public IASTCompilationResult<TNumber> Compile(IASTCompilationContext<TNumber> ctx, DSLFunctionDefinition toCompile)
        {
            var product = _base.Compile(ctx, toCompile);

            if (product is not ASTCompilationResult<TNumber> ret)
                return product;

            return new ASTCompilationResult<TNumber>(ret.Expression.Autocached(), ret.ThisFunctionWrapper);
        }

    }
}
