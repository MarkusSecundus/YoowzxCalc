using Antlr4.Runtime;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.UnitTests
{
    internal class LexerTests
    {
        private CalculatorDSLLexer lex(string s) => new CalculatorDSLLexer(new AntlrInputStream(s));

        [Test]
        public void SkipsWhitespace()
        {
            CalculatorDSLLexer l = lex("  A     reewr  ");
            
        }
    }
}
