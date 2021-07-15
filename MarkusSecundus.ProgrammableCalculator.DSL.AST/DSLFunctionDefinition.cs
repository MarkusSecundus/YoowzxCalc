using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public sealed class DSLFunctionDefinition
    {
        public string Name { get; init; }

        public IReadOnlyList<string> Arguments { get; init; }

        public DSLExpression Body { get; init; }
    }
}
