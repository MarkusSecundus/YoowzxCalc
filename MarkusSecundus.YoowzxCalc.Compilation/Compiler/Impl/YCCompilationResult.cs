using MarkusSecundus.Util;
using System;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCCompilationResult<TNumber> : IYCCompilationResult<TNumber>
    {
        public YCCompilationResult(Delegate expression, SettableOnce<Delegate> thisFunctionWrapper)
            => (Expression, ThisFunctionWrapper) = (expression, thisFunctionWrapper);

        internal Delegate Expression { get; }
        internal SettableOnce<Delegate> ThisFunctionWrapper { get; }

        public TDelegate Finalize<TDelegate>() where TDelegate : Delegate
            => (ThisFunctionWrapper.Value = Expression) as TDelegate;

        public Delegate Finalize() => Finalize<Delegate>();
    }
}
