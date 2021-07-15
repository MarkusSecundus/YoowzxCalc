using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions.Throwers;
using System;
using System.IO;

namespace MarkusSecundus.ProgrammableCalculator.DSL.Parser
{
    class ASTBuilder : IASTBuilder
    {
        public DSLFunctionDefinition Build(string source) => Build(new AntlrInputStream(source));
        public DSLFunctionDefinition Build(Stream source) => Build(new AntlrInputStream(source));
        public DSLFunctionDefinition Build(TextReader source) => Build(new AntlrInputStream(source));


        private DSLFunctionDefinition Build(AntlrInputStream source)
        {
            CalculatorDSLLexer lexer = new CalculatorDSLLexer(source);
            CalculatorDSLParser parser = new CalculatorDSLParser(new CommonTokenStream(lexer));

            lexer.AddErrorListener(LexicalErrorListener.Instance);
            parser.AddErrorListener(SyntaxErrorListener.Instance);


            var tree = parser.unit().ToStringTree();
            Console.WriteLine(tree);
            return null;
        }


    }


    class ASTBuilderVisitor : CalculatorDSLBaseVisitor<DSLExpression>
    {
        public override DSLExpression VisitAdd_expression([NotNull] CalculatorDSLParser.Add_expressionContext context)
        {
            return null;
        }

        public override DSLExpression VisitArgs_list([NotNull] CalculatorDSLParser.Args_listContext context)
        {
            return base.VisitArgs_list(context);
        }

        public override DSLExpression VisitBracketed_args_list__opt([NotNull] CalculatorDSLParser.Bracketed_args_list__optContext context)
        {
            return base.VisitBracketed_args_list__opt(context);
        }

        public override DSLExpression VisitBracketed_invoke_list__opt([NotNull] CalculatorDSLParser.Bracketed_invoke_list__optContext context)
        {
            return base.VisitBracketed_invoke_list__opt(context);
        }

        public override DSLExpression VisitErrorNode([NotNull] IErrorNode node)
        {
            return base.VisitErrorNode(node);
        }

        public override DSLExpression VisitExpression([NotNull] CalculatorDSLParser.ExpressionContext context)
        {
            return context.add_expression().Accept(this);
        }

        public override DSLExpression VisitFunction_call([NotNull] CalculatorDSLParser.Function_callContext context)
        {
            return base.VisitFunction_call(context);
        }

        public override DSLExpression VisitFunction_definition([NotNull] CalculatorDSLParser.Function_definitionContext context)
        {
            return base.VisitFunction_definition(context);
        }

        public override DSLExpression VisitInvoke_list([NotNull] CalculatorDSLParser.Invoke_listContext context)
        {
            return base.VisitInvoke_list(context);
        }

        public override DSLExpression VisitLiteral([NotNull] CalculatorDSLParser.LiteralContext context)
        {

            return base.VisitLiteral(context);
        }

        public override DSLExpression VisitMult_expression([NotNull] CalculatorDSLParser.Mult_expressionContext context)
        {
            return base.VisitMult_expression(context);
        }

        public override DSLExpression VisitPow_expression([NotNull] CalculatorDSLParser.Pow_expressionContext context)
        {
            return base.VisitPow_expression(context);
        }

        public override DSLExpression VisitTerminal([NotNull] ITerminalNode node)
        {
            switch (node.Symbol.Type)
            {
                case CalculatorDSLLexer.IDENTIFIER:
                    return new DSLArgumentExpression { ArgumentName = node.Symbol.Text };
                case CalculatorDSLLexer.NUMBER:
                    return new DSLConstantExpression { Value = node.Symbol.Text };
            }
            return null;
        }

        public override DSLExpression VisitUnit([NotNull] CalculatorDSLParser.UnitContext context)
        {
            return base.VisitUnit(context);
        }
    }
}
