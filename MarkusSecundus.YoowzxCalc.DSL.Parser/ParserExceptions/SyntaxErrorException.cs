using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions
{
    public class SyntaxErrorException : SyntaxAndLexicalErrorExceptionBase<IToken>
    {
        public SyntaxErrorException(string message, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine) 
            : base(message,recognizer, offendingSymbol, line, charPositionInLine) {}
    }
}
