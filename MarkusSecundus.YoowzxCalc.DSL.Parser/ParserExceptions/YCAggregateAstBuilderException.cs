using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions
{
    public sealed class YCAggregateAstBuilderException : YCAstBuilderException
    {
        private YCAggregateAstBuilderException(IReadOnlyList<YCLexicalErrorException> lexicalErrors, IReadOnlyList<YCSyntaxErrorException> syntaxErrors)
            :base(lexicalErrors.Chain<YCAstBuilderException>(syntaxErrors).Select(e => e.Message).MakeString("\n\n"))
        {
            (LexicalErrors, SyntaxErrors) = (lexicalErrors, syntaxErrors);
        }

        public IReadOnlyList<YCLexicalErrorException> LexicalErrors { get; }

        public IReadOnlyList<YCSyntaxErrorException> SyntaxErrors { get; }



        public static YCAggregateAstBuilderException Empty { get; } = new(CollectionsUtils.EmptyList<YCLexicalErrorException>(), CollectionsUtils.EmptyList<YCSyntaxErrorException>());


        internal class Builder
        {
            private readonly List<YCLexicalErrorException> _lexicalErrors = new();
            private readonly List<YCSyntaxErrorException> _syntaxErrors = new();

            public bool IsEmpty => (_lexicalErrors.Count | _syntaxErrors.Count) == 0;

            internal IAntlrErrorListener<int> LexicalErrorListener => new _LexicalErrorListener(this);
            internal IAntlrErrorListener<IToken> SyntaxErrorListener => new _SyntaxErrorListener(this);


            public YCAggregateAstBuilderException Build() => new (_lexicalErrors, _syntaxErrors);


            private sealed class _LexicalErrorListener : IAntlrErrorListener<int>
            {
                private readonly Builder _base;

                public _LexicalErrorListener(Builder @base) => _base = @base;


                public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
                    => _base._lexicalErrors.Add(new YCLexicalErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine));
            }

            private sealed class _SyntaxErrorListener : IAntlrErrorListener<IToken>
            {
                private readonly Builder _base;

                public _SyntaxErrorListener(Builder @base) => _base = @base;

                public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
                    => _base._syntaxErrors.Add(new YCSyntaxErrorException(msg, recognizer, offendingSymbol, line, charPositionInLine));
            }
        }


    }
}
