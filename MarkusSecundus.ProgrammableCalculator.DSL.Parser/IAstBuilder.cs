using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser
{
    public interface IASTBuilder
    {
        public DSLFunctionDefinition Build(string source);
        public DSLFunctionDefinition Build(Stream source);
        public DSLFunctionDefinition Build(TextReader source);
    }
}
