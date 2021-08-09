using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    public class ASTInterpretationException : Exception
    {
        public ASTInterpretationException() { }
        public ASTInterpretationException(string message) : base(message) { }
    }
}
