using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public interface IASTCompilationResult<TNumber>
    {
        public Delegate Compile() => Compile<Delegate>();
        public TDelegate Compile<TDelegate>() where TDelegate : Delegate;
    }
}
