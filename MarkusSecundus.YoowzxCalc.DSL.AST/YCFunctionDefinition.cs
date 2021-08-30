using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;




namespace MarkusSecundus.YoowzxCalc.DSL.AST
{
    public sealed record YCFunctionDefinition
    {
        public string Name { get; init; }

        public IReadOnlyList<string> Arguments { get; init; }

        public IReadOnlyDictionary<string, string> Annotations { get; init; } = CollectionsUtils.EmptyDictionary<string, string>();

        public YCExpression Body { get; init; }



        public bool IsAnonymous => object.ReferenceEquals(Name, AnonymousFunctionName);

        public static readonly string AnonymousFunctionName = new string("<#anonymous>");
        public static readonly string EmptyAnnotationValue = new string(string.Empty);




        public static string HeadRepr(YCFunctionDefinition self) => $"{self.Name}({self.Arguments.MakeString()})";


        public override string ToString() => $"{HeadRepr(this)} := {Body}";


        public override int GetHashCode() => (Name, Arguments.SequenceHashCode(), Body).GetHashCode();
    }
}
