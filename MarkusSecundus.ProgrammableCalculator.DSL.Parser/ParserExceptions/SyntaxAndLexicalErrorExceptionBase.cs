using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions
{
    public class SyntaxAndLexicalErrorExceptionBase<TToken> : Exception
    {

        public SyntaxAndLexicalErrorExceptionBase(string message, IRecognizer recognizer, TToken offendingSymbol, int line, int charPositionInLine)
            : base(createMessage(message, offendingSymbol, line, charPositionInLine))
        {
            CoreMessage = message;
            Recognizer = recognizer;
            OffendingSymbol = offendingSymbol;
            Line = line;
            CharPositionInLine = charPositionInLine;
        }

        public IRecognizer Recognizer { get; init; }
        public TToken OffendingSymbol { get; init; }
        public int Line { get; init; }
        public int CharPositionInLine { get; init; }

        public string CoreMessage { get; }

        private static string createMessage(string message, TToken offendingSymbol, int line, int charPositionInLine)
            => $"On line {line}, position {charPositionInLine}, symbol '{offendingSymbol}': {message}";
    }
}
