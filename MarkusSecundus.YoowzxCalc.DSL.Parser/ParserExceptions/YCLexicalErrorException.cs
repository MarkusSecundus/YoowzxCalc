using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions
{
    public class YCLexicalErrorException : YCSyntaxAndLexicalErrorExceptionBase<int>
    {
        public YCLexicalErrorException(string message, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine)
            : base(message,recognizer, offendingSymbol, line, charPositionInLine) {}
    }
}
