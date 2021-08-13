using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions
{
    public class ParserException : FormatException
    {
        public ParserException() : base() { }
        public ParserException(string message) : base(message) { }
    }
}
