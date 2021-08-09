using MarkusSecundus.Util;
using System;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class ASTCompilationResult<TNumber> : IASTCompilationResult<TNumber>
    {
        public ASTCompilationResult(Delegate expression, SettableOnce<Delegate> thisFunctionWrapper)
            => (Expression, ThisFunctionWrapper) = (expression, thisFunctionWrapper);

        internal Delegate Expression { get; }
        internal SettableOnce<Delegate> ThisFunctionWrapper { get; }

        public TDelegate Compile<TDelegate>() where TDelegate : Delegate
            => (ThisFunctionWrapper.Value = Expression) as TDelegate;

        public Delegate Compile() => Compile<Delegate>();
    }
}
