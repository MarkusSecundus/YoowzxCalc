using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{
    public class ASTInterpretationException : Exception
    {
        public ASTInterpretationException() { }
        public ASTInterpretationException(string message):base(message) { }
    }
}
