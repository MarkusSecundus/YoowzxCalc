using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions
{
    public class YCAstBuilderException : FormatException
    {
        public YCAstBuilderException() : base() { }
        public YCAstBuilderException(string message) : base(message) { }
    }
}
