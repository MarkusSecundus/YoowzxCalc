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



    /// <summary>
    /// Root of every YoowzxCalc abstract expression tree. 
    /// Describes a complete function definition.
    /// </summary>
    public sealed record YCFunctionDefinition
    {
        /// <summary>
        /// Name of the function
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Names of all the arguments in order from left to right
        /// </summary>
        public IReadOnlyList<string> Arguments { get; init; }

        /// <summary>
        /// Annotations applied to the function.
        /// </summary>
        public IReadOnlyDictionary<string, string> Annotations { get; init; } = CollectionsUtils.EmptyDictionary<string, string>();

        /// <summary>
        /// Expression representing the actual function body.
        /// </summary>
        public YCExpression Body { get; init; }


        /// <summary>
        /// If the function has its own actual name or if it is just a placeholder.
        /// </summary>
        public bool IsAnonymous => object.ReferenceEquals(Name, AnonymousFunctionName);

        /// <summary>
        /// Constant to be used as name placeholder for all anonymous functions.
        /// </summary>
        public static readonly string AnonymousFunctionName = new string("<#anonymous>");

        /// <summary>
        /// Constant to be used as placeholder for all functions with no annotations.
        /// </summary>
        public static readonly string EmptyAnnotationValue = new string(string.Empty);



        /// <summary>
        /// Get text representation for the function's signature.
        /// </summary>
        /// <param name="self">The definition whose header to get.</param>
        /// <returns>Text representation of the specified function's signature.</returns>
        public static string HeadRepr(YCFunctionDefinition self) => $"{self.Name}({self.Arguments.MakeString()})";


        public override string ToString() => $"{HeadRepr(this)} := {Body}";


        public override int GetHashCode() => (Name, Arguments.SequenceHashCode(), Body).GetHashCode();
    }
}
