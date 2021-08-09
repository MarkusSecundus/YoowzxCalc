using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions
{
    public sealed class AggregateParserException : ParserException
    {
        private AggregateParserException(IReadOnlyList<LexicalErrorException> lexicalErrors, IReadOnlyList<SyntaxErrorException> syntaxErrors)
            :base(lexicalErrors.Chain<ParserException>(syntaxErrors).Select(e => e.Message).MakeString("\n\n"))
        {
            (LexicalErrors, SyntaxErrors) = (lexicalErrors, syntaxErrors);
        }

        public IReadOnlyList<LexicalErrorException> LexicalErrors { get; }

        public IReadOnlyList<SyntaxErrorException> SyntaxErrors { get; }



        public static AggregateParserException Empty { get; } = new(CollectionsUtils.EmptyList<LexicalErrorException>(), CollectionsUtils.EmptyList<SyntaxErrorException>());


        internal class Builder
        {
            private readonly List<LexicalErrorException> _lexicalErrors = new();
            private readonly List<SyntaxErrorException> _syntaxErrors = new();

            public bool IsEmpty => (_lexicalErrors.Count | _syntaxErrors.Count) == 0;

            internal IAntlrErrorListener<int> LexicalErrorListener => new _LexicalErrorListener(this);
            internal IAntlrErrorListener<IToken> SyntaxErrorListener => new _SyntaxErrorListener(this);


            public AggregateParserException Build() => new (_lexicalErrors, _syntaxErrors);


            private sealed class _LexicalErrorListener : IAntlrErrorListener<int>
            {
                private readonly Builder _base;

                public _LexicalErrorListener(Builder @base) => _base = @base;


                public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
                    => _base._lexicalErrors.Add(new LexicalErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine));
            }

            private sealed class _SyntaxErrorListener : IAntlrErrorListener<IToken>
            {
                private readonly Builder _base;

                public _SyntaxErrorListener(Builder @base) => _base = @base;

                public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
                    => _base._syntaxErrors.Add(new SyntaxErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine));
            }
        }


    }
}
