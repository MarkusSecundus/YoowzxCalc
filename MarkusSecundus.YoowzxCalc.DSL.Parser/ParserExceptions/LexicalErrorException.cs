using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions
{
    public class LexicalErrorException : SyntaxAndLexicalErrorExceptionBase<int>
    {
        public LexicalErrorException(string message, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine)
            : base(message,recognizer, offendingSymbol, line, charPositionInLine) {}
    }
}
