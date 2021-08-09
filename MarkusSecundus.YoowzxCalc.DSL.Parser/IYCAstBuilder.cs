using MarkusSecundus.YoowzxCalc.DSL.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly : CLSCompliant(false)]

namespace MarkusSecundus.YoowzxCalc.DSL.Parser
{
    public interface IYCAstBuilder
    {
        public static IYCAstBuilder Instance { get; } = new YCAstBuilder();

        public YCFunctionDefinition Build(string source);
        public YCFunctionDefinition Build(Stream source);
        public YCFunctionDefinition Build(TextReader source);
    }
}
