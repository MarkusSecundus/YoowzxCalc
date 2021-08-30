using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions
{
    public class YCSyntaxErrorException : YCSyntaxAndLexicalErrorExceptionBase<IToken>
    {
        public YCSyntaxErrorException(string message, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine) 
            : base(message,recognizer, offendingSymbol, line, charPositionInLine) {}
    }
}
