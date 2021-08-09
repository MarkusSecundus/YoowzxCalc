using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public class YCCompilerException : Exception
    {
        public YCCompilerException() { }
        public YCCompilerException(string message) : base(message) { }
    }
}
