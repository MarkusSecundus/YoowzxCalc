using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.Parser.ParserExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly : CLSCompliant(false)]

namespace MarkusSecundus.YoowzxCalc.DSL.Parser
{
    /// <summary>
    /// Object responsible for parsing a text stream representing a mathematical expression into its abstract syntax tree.
    /// </summary>
    public interface IYCAstBuilder
    {
        /// <summary>
        /// Instance of canonical implementation.
        /// 
        /// Stateless, parses according to the grammar described in <see cref="MarkusSecundus.YoowzxCalc.DSL.AST.YCFunctionDefinition"/>.
        /// </summary>
        public static IYCAstBuilder Instance { get; } = new YCAstBuilder();

        /// <summary>
        /// Parses expression from provided string.
        /// </summary>
        /// <param name="source">Text to parse</param>
        /// <exception cref="YCAggregateAstBuilderException">Encompassing all lexer and parser errors that have occured</exception>
        /// <returns>AST describing the expression</returns>
        public YCFunctionDefinition Build(string source);

        /// <summary>
        /// Parses expression from provided Stream.
        /// </summary>
        /// <param name="source">Character sequence to parse</param>
        /// <exception cref="YCAggregateAstBuilderException">Encompassing all lexer and parser errors that have occured</exception>
        /// <returns>AST describing the expression</returns>
        public YCFunctionDefinition Build(Stream source);

        /// <summary>
        /// Parses expression from provided TextReader.
        /// </summary>
        /// <param name="source">Character sequence to parse</param>
        /// <exception cref="YCAggregateAstBuilderException">Encompassing all lexer and parser errors that have occured</exception>
        /// <returns>AST describing the expression</returns>
        public YCFunctionDefinition Build(TextReader source);
    }
}
