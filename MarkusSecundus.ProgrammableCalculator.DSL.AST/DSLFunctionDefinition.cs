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
        public T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx) => visitor.Visit(this, ctx);
        
        
        public string Name { get; init; }

        public IReadOnlyList<string> Arguments { get; init; }

        public DSLExpression Body { get; init; }



        public bool IsAnonymous => object.ReferenceEquals(Name, AnonymousFunctionName);

        public static readonly string AnonymousFunctionName = new string("<#anonymous>");





        public static string HeadRepr(DSLFunctionDefinition self) => $"{self.Name}({self.Arguments.Concat()})";


        public override string ToString() => $"{HeadRepr(this)} := {Body}";

        public override bool Equals(object obj) => obj is DSLFunctionDefinition e && Name == e.Name && Arguments.SequenceEqual(e.Arguments) && Equals(Body, e.Body);

        public override int GetHashCode() => (Name, Arguments.SequenceHashCode(), Body).GetHashCode();
    }
}
