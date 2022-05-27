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
            CollectionAssert.AreEqual(lex(text).GetAllTokens().Select(t => t.Type), types);
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
        public void WhitespacePreservedInString()
        {
            const string inner = "'Toto je  text  '", s = " \t   " + inner + "\n";

            SingleTokenParsedAssert(s, Lex.IDENTIFIER, inner);
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
        public void AllowsScientificNumberNotation()
        {
            SingleTokenParsedAssert("103.432e123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E+123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("103.432E-123", Lex.IDENTIFIER);
            SingleTokenParsedAssert("1E-123", Lex.IDENTIFIER);
        }

    }
}
