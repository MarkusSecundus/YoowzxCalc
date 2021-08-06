using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTCompilationResult<TNumber> : IASTCompilationResult<TNumber>
    {
        public ASTCompilationResult(LambdaExpression expression, SettableOnce<Delegate> thisFunctionWrapper)
            => (Expression, ThisFunctionWrapper) = (expression, thisFunctionWrapper);

        internal LambdaExpression Expression { get; }
        internal SettableOnce<Delegate> ThisFunctionWrapper { get; }

        public TDelegate Compile<TDelegate>() where TDelegate: Delegate
            => (ThisFunctionWrapper.Value = Expression.Compile()) as TDelegate;

        public Delegate Compile() => Compile<Delegate>();
    }
}
