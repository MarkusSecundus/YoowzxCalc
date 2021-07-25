using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public sealed class DSLFunctionDefinition
    {
        public T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);
        
        
        public string Name { get; init; }

        public bool IsAnonymous => object.ReferenceEquals(Name, AnonymousFunctionName);

        public IReadOnlyList<string> Arguments { get; init; }

        public DSLExpression Body { get; init; }


        public override string ToString() => $"{Name}({Arguments.Concat()}) := {Body}";


        public static readonly string AnonymousFunctionName = new string("<#anonymous>");
    }
}
