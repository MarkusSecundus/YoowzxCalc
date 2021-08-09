using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions.Throwers
{
    sealed class LexicalErrorListener : IAntlrErrorListener<int>
    {
        private LexicalErrorListener() { }

        public static LexicalErrorListener Instance { get; } = new();


        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new LexicalErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine);
        }
    }
}
