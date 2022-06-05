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
        public string Name { get; init; } = AnonymousFunctionName;

        /// <summary>
        /// Names of all the arguments in order from left to right
        /// </summary>
        public IReadOnlyList<string> Arguments 
        { 
            get => _arguments; 
            init => _arguments = value is ListComparedByContents<string>e
                ? e
                : new ListComparedByContents<string>(value); 
        }
        private ListComparedByContents<string> _arguments = EmptyArguments;

        /// <summary>
        /// Annotations applied to the function.
        /// </summary>
        public IReadOnlyDictionary<string, string> Annotations 
        { 
            get => _annotations; 
            init => _annotations = value is DictionaryComparedByContents<string, string> d
                ? d
                : new DictionaryComparedByContents<string, string>(value); 
        }
        private DictionaryComparedByContents<string, string> _annotations = EmptyAnnotations;


        /// <summary>
        /// Expression representing the actual function body.
        /// </summary>
        public YCExpression Body { get; init; }


        /// <summary>
        /// If the function has its own actual name or if it is just a placeholder.
        /// </summary>
        public bool IsAnonymous => object.ReferenceEquals(Name, AnonymousFunctionName);

        /// <summary>
        /// Determines whether the given annotation value represents an empty annotation.
        /// </summary>
        /// <param name="annotationValue">Value of an annotation to be checked for emptiness</param>
        /// <returns>Whether the given annotation has a value</returns>
        public bool AnnotationValueIsEmpty(string annotationValue) => ReferenceEquals(annotationValue, EmptyAnnotationValue);

        /// <summary>
        /// Constant to be used as name placeholder for all anonymous functions.
        /// Function f is anonymous iff <c>ReferenceEquals(AnonymousFunctionName, f.Name)</c>
        /// </summary>
        public static readonly string AnonymousFunctionName = new string("<#anonymous>");

        /// <summary>
        /// Constant to be used as placeholder for annotations without a value.
        /// Annotation n of function f is empty iff <c>ReferenceEquals(EmptyAnnotationValue, f.Annotations[n])</c>
        /// </summary>
        public static readonly string EmptyAnnotationValue = new string(string.Empty);



        /// <summary>
        /// Get text representation for the function's signature.
        /// </summary>
        /// <param name="self">The definition whose header to get.</param>
        /// <returns>Text representation of the specified function's signature.</returns>
        public static string HeadRepr(YCFunctionDefinition self) => $"{self.Name}({self.Arguments.MakeString()})";


        /// <inheritdoc/>
        public override string ToString() => $"{HeadRepr(this)} := {Body}";


        /// <inheritdoc/>
        public override int GetHashCode() => (Name, Arguments.SequenceHashCode(), Body).GetHashCode();



        private static readonly ListComparedByContents<string> EmptyArguments = new ListComparedByContents<string>(CollectionsUtils.EmptyList<string>());
        private static readonly DictionaryComparedByContents<string, string> EmptyAnnotations = new DictionaryComparedByContents<string, string>(CollectionsUtils.EmptyDictionary<string, string>());
    }
}
