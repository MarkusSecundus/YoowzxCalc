using Antlr4.Runtime;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lex = MarkusSecundus.YoowzxCalc.DSL.Parser.CalculatorDSLLexer;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.UnitTests
{
    internal class LexerTests
    {
        private Lex lex(string s) => new Lex(new AntlrInputStream(s));

        private void AssertTokensEqual(string text, params int[] types)
        {
            CollectionAssert.AreEqual(types, lex(text).GetAllTokens().Select(t => t.Type));
        }

        private void SingleTokenParsedAssert(string text, int type, string inner=null)
        {
            AssertTokensEqual(text, type);
            Assert.AreEqual(inner??=text, lex(text).NextToken().Text);
        }






        [Test]
        public void SkipsWhitespace()
        {
            AssertTokensEqual($"  \n \t {(char)7}  ");


            var bld = new StringBuilder();
            for (int t = ' '; t >= 0; --t) //any char <= ' ' (in ASCII) is considered whitespace
                bld.Append((char)t);

            AssertTokensEqual(bld.ToString());
        }


        [Test]
        public void WhitespaceIsDelimiter()
        {
            AssertTokensEqual(">  =", Lex.COMPARE_GT, Lex.COMPARE_EQ);

            AssertTokensEqual("dsa re re", Lex.IDENTIFIER, Lex.IDENTIFIER, Lex.IDENTIFIER);
        }

        [Test]
        public void BasicIdentifierFormat()
        {
            SingleTokenParsedAssert("_", Lex.IDENTIFIER);
            SingleTokenParsedAssert("__", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdef", Lex.IDENTIFIER);
            SingleTokenParsedAssert("_abcdef", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdef_", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdefghijklmnopqrstuvwxyz", Lex.IDENTIFIER);
            SingleTokenParsedAssert("ABCDEF", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdefABCDEF", Lex.IDENTIFIER);
            SingleTokenParsedAssert("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", Lex.IDENTIFIER);
            SingleTokenParsedAssert("abcdef0123456789_", Lex.IDENTIFIER);
            SingleTokenParsedAssert("_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_", Lex.IDENTIFIER);
        }
        [Test]
        public void UnicodeCharsAreValidPartOfIdentifier()
        {
            SingleTokenParsedAssert("řžáýÍÁÝ43eěŽŘČŘČú_end", Lex.IDENTIFIER);

            var bld = new StringBuilder();
            for (int i = 257; i < 2000; i += 1) //all unicode chars are - trying with a reasonable subset to not totally wreck performance
                bld.Append((char)i);
            SingleTokenParsedAssert(bld.ToString(), Lex.IDENTIFIER);
        }


        [Test]
        public void BasicNumberFormat()
        {
            SingleTokenParsedAssert("0123456789", Lex.IDENTIFIER);
            SingleTokenParsedAssert("9876543210", Lex.IDENTIFIER);
            SingleTokenParsedAssert("0123456789.0123456789", Lex.IDENTIFIER);
            SingleTokenParsedAssert("0123456789.01234567897894561230890465", Lex.IDENTIFIER);
            SingleTokenParsedAssert("0123456789.", Lex.IDENTIFIER);
            SingleTokenParsedAssert("0", Lex.IDENTIFIER);
            SingleTokenParsedAssert("1.1", Lex.IDENTIFIER);
        }
        [Test]
        public void NumbersAllowScientificNotation()
        {
            SingleTokenParsedAssert("103.432e123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E+123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E-123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("1E-123", Lex.IDENTIFIER);
        }

        [Test]
        public void WhitespacePreservedInString()
        {
            string inner;

            inner = "'Toto je  text  '";
            SingleTokenParsedAssert($" \t   {inner}\n ", Lex.IDENTIFIER, inner);

            inner = "\"Toto je  text  \"";
            SingleTokenParsedAssert($" \t   {inner}\n ", Lex.IDENTIFIER, inner);
        }
        [Test]
        public void SpecialCharsAreNotSpecialInString()
        {
            SingleTokenParsedAssert("'Toto je symbol: ?%12+[]  '", Lex.IDENTIFIER);
        }

        [Test]
        public void StringsSupportQuoteEscaping()
        {
            SingleTokenParsedAssert("'Escaped quote: \\' '", Lex.IDENTIFIER);
            SingleTokenParsedAssert("\"Escaped quote: \\\" \"", Lex.IDENTIFIER);
        }


        [Test]
        public void WhitespacelessStreamsSeparatedInNaturalWay()
        {
            AssertTokensEqual(
                "aa+=:=(1e+13:'This is a text string!'[)",
                Lex.IDENTIFIER, Lex.PLUS, Lex.COMPARE_EQ, Lex.ASSIGN, Lex.LPAR, Lex.IDENTIFIER, Lex.COLON, Lex.IDENTIFIER, Lex.LBRA, Lex.RPAR
            );
        }


        [Test]
        public void CompositeIdentifiers()
        {
            SingleTokenParsedAssert("@'Toto je text'", Lex.IDENTIFIER);
            SingleTokenParsedAssert("$\"Toto je text\"", Lex.IDENTIFIER);
            SingleTokenParsedAssert("143l", Lex.IDENTIFIER);
            SingleTokenParsedAssert("std::cout.value1", Lex.IDENTIFIER);
            SingleTokenParsedAssert("'This is a text string'::type132E-31", Lex.IDENTIFIER);
            SingleTokenParsedAssert("\"\"'''Te  x t'132@'rew'\"  ?:+ \"4.rew23.4332'+'", Lex.IDENTIFIER);
        }
    }
}
