using Antlr4.Runtime;
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
    /// 
    /// <para/>
    /// Canonically the grammar should look like this:
    /// <para/>
    /// WHITESPACE: [\u0000- ] -&gt; ignored 
    /// <para/>
    /// IDENTIFIER: (any_non_special_non_whitespace_char 
    ///              | ('"'(any_char_except_double_quote | '\\"')'"')      //string in double-quotes, double-quotes inside it must be escaped
    ///              | ('\''(any_char_except_single_quote | '\\\'')'\'')   //string in single-quotes, single-quotes inside it must be escaped
    ///              | (DIGIT+('.'DIGIT*([eE][+-]DIGIT+))?  )                       //any number with +/- allowed inside E-notation suffix ([+-] are special characters)
    ///              )+
    ///       
    /// 
    /// <para/>
    /// function_definition: annotations_list? (function_header ':=')? expression 
    /// <para/>
    /// 
    /// 
    /// expression : literal
    ///     | unary_expression
    ///     | binary_expression
    ///     | ternary_expression
    ///     | functioncall
    ///     
    /// <para/>
    /// 
    /// literal : IDENTIFIER
    ///     | '(' expression ')'
    ///     
    /// <para/>
    ///     
    ///     -- with usual operator precedence
    /// <para/>
    /// unary_expression: ('+' | '-' | ('!' | '¬') ) expression 
    /// <para/>
    /// binary_expression: expression ( '+' | '-' | '*' | '/' | '%' | ('**' | '^') | ('∧'| '&amp;') | ('|' | '∨') | '&lt;' | '&lt;=' | '&gt;' | '&gt;=' | ('=' | '==') | '!=' ) expression 
    /// <para/>
    /// ternary_expression: expression '?' expression ':' expression 
    /// <para/>
    /// 
    /// functioncall: IDENTIFIER '(' (expression (',' expression)* )? ')'
    /// <para/>
    /// 
    /// 
    /// function_header: IDENTIFIER '(' (IDENTIFIER (',' IDENTIFIER)* )? ')'
    /// <para/>
    /// 
    /// annotations_list: '[' (annotation (',' annotation)* )? ']'
    /// <para/>
    /// annotation: IDENTIFIER (':' IDENTIFIER)?
    /// <para/>
    /// 
    /// </summary>
    public interface IYCAstBuilder
    {
        /// <summary>
        /// Instance of canonical implementation.
        /// 
        /// Stateless, parses according to the grammar described in <see cref="IYCAstBuilder"/>.
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

        /// <summary>
        /// Parses expression.
        /// Allows for any custom lexer with the expected output token format (for cannonical lexer see <see cref="CalculatorDSLLexer"/>).
        /// </summary>
        /// <param name="lexer">Custom lexer providing the stream of tokens to parse</param>
        /// <exception cref="YCAggregateAstBuilderException">Encompassing all lexer and parser errors that have occured</exception>
        /// <returns>AST describing the expression</returns>
        public YCFunctionDefinition Build(Lexer lexer);
    }
}
