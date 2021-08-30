using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions.Throwers
{
    sealed class SyntaxErrorListener : IAntlrErrorListener<IToken>
    {
        private SyntaxErrorListener() { }

        public static SyntaxErrorListener Instance { get; } = new();

        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new YCSyntaxErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine);
        }
    }
}
