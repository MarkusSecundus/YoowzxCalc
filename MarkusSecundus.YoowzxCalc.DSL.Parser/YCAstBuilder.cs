using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions;
using MarkusSecundus.ProgrammableCalculator.DSL.Parser.ParserExceptions.Throwers;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using MarkusSecundus.YoowzxCalc.DSL.AST.BinaryExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.OtherExpressions;
using MarkusSecundus.YoowzxCalc.DSL.AST.PrimaryExpression;
using MarkusSecundus.YoowzxCalc.DSL.AST.UnaryExpressions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace MarkusSecundus.YoowzxCalc.DSL.Parser
{
    class YCAstBuilder : IYCAstBuilder
    {
        public YCFunctionDefinition Build(string source) => Build(new AntlrInputStream(source));
        public YCFunctionDefinition Build(Stream source) => Build(new AntlrInputStream(source));
        public YCFunctionDefinition Build(TextReader source) => Build(new AntlrInputStream(source));


        private static YCFunctionDefinition Build(AntlrInputStream source)
        {
            CalculatorDSLLexer lexer = new CalculatorDSLLexer(source);
            CalculatorDSLParser parser = new CalculatorDSLParser(new CommonTokenStream(lexer));

            AggregateParserException.Builder e = new();

            lexer.AddErrorListener(e.LexicalErrorListener);
            parser.AddErrorListener(e.SyntaxErrorListener);

            var l = new ParserListener();

            parser.AddParseListener(l);

            try
            {
                parser.unit();
            }
            catch { }

            if (!e.IsEmpty)
                throw e.Build();

            if (l.ReturnValue == null)
                throw new ParserException("Parsing failed for some reason!");

            return l.ReturnValue;
        }


    }



    sealed class ParserListener : CalculatorDSLBaseListener
    {
        public YCFunctionDefinition ReturnValue { get; private set; }

        private readonly List<List<string>> argsStack = new();
        private readonly List<List<YCExpression>> invokeArgsStack = new();
        private readonly List<YCExpression> stack = new();
        private readonly List<Dictionary<string, string>> annotationSetsStack = new();
        private readonly List<(string Key, string Value)> annotationsStack = new();

        private void pushBinary<T>() where T: YCBinaryExpression, new()
            => stack.Push(new T { RightChild = stack.Pop(), LeftChild = stack.Pop() });

        private void pushUnary<T>() where T : YCUnaryExpression, new()
            => stack.Push(new T { Child = stack.Pop() });








        public override void ExitIdentifier_expr([NotNull] CalculatorDSLParser.Identifier_exprContext context)
            => stack.Push(new YCLiteralExpression { Value = context.IDENTIFIER().Symbol.Text });



        public override void ExitUnary_minus_expr([NotNull] CalculatorDSLParser.Unary_minus_exprContext context)
            => pushUnary<YCUnaryMinusExpression>();

        public override void ExitUnary_plus_expr([NotNull] CalculatorDSLParser.Unary_plus_exprContext context)
            => pushUnary<YCUnaryPlusExpression>();

        public override void ExitLogical_not_expr([NotNull] CalculatorDSLParser.Logical_not_exprContext context)
            => pushUnary<YCUnaryLogicalNotExpression>();



        public override void ExitAdd_expr([NotNull] CalculatorDSLParser.Add_exprContext context)
            => pushBinary<YCAddExpression>();

        public override void ExitSubtract_expr([NotNull] CalculatorDSLParser.Subtract_exprContext context)
            => pushBinary<YCSubtractExpression>();

        public override void ExitMult_expr([NotNull] CalculatorDSLParser.Mult_exprContext context)
            => pushBinary<YCMultiplyExpression>();

        public override void ExitDiv_expr([NotNull] CalculatorDSLParser.Div_exprContext context)
            => pushBinary<YCDivideExpression>();

        public override void ExitMod_expr([NotNull] CalculatorDSLParser.Mod_exprContext context)
            => pushBinary<YCModuloExpression>();

        public override void ExitExponent_expr([NotNull] CalculatorDSLParser.Exponent_exprContext context)
            => pushBinary<YCExponentialExpression>();





        public override void ExitLt_expr([NotNull] CalculatorDSLParser.Lt_exprContext context)
            => pushBinary<YCCompareLessThanExpression>();

        public override void ExitLe_expr([NotNull] CalculatorDSLParser.Le_exprContext context)
            => pushBinary<YCCompareLessOrEqualExpression>();

        public override void ExitGt_expr([NotNull] CalculatorDSLParser.Gt_exprContext context)
            => pushBinary<YCCompareGreaterThanExpression>();

        public override void ExitGe_expr([NotNull] CalculatorDSLParser.Ge_exprContext context)
            => pushBinary<YCCompareGreaterOrEqualExpression>();


        public override void ExitEq_expr([NotNull] CalculatorDSLParser.Eq_exprContext context)
            => pushBinary<YCCompareIsEqualExpression>();

        public override void ExitNe_expr([NotNull] CalculatorDSLParser.Ne_exprContext context)
            => pushBinary<YCCompareIsNotEqualExpression>();



        public override void ExitAnd_expr([NotNull] CalculatorDSLParser.And_exprContext context)
            => pushBinary<YCLogicalAndExpression>();

        public override void ExitOr_expr([NotNull] CalculatorDSLParser.Or_exprContext context)
            => pushBinary<YCLogicalOrExpression>();

        public override void ExitTernary_expr([NotNull] CalculatorDSLParser.Ternary_exprContext context)
            => stack.Push(new YCConditionalExpression { IfFalse = stack.Pop(), IfTrue = stack.Pop(), Condition = stack.Pop() });















        public override void ExitFunctioncall_expr([NotNull] CalculatorDSLParser.Functioncall_exprContext context)
            => stack.Push(new YCFunctioncallExpression { Name = context.IDENTIFIER().Symbol.Text, Arguments = invokeArgsStack.Pop() });


        public override void ExitInvoke_list_create([NotNull] CalculatorDSLParser.Invoke_list_createContext context)
            => invokeArgsStack.Push(new List<YCExpression> { stack.Pop() });

        public override void ExitInvoke_list_add([NotNull] CalculatorDSLParser.Invoke_list_addContext context)
            => invokeArgsStack.Peek().Add(stack.Pop());

        public override void ExitBracketed_invoke_list_empty([NotNull] CalculatorDSLParser.Bracketed_invoke_list_emptyContext context)
            => invokeArgsStack.Push(new List<YCExpression>());





        public override void ExitFunction_definition_has_no_annotations([NotNull] CalculatorDSLParser.Function_definition_has_no_annotationsContext context)
            => ReturnValue = new YCFunctionDefinition { Name = context.IDENTIFIER().Symbol.Text, Arguments = argsStack.Pop(), Body = stack.Pop() };

        public override void ExitFunction_definition_has_annotations([NotNull] CalculatorDSLParser.Function_definition_has_annotationsContext context)
            => ReturnValue = new YCFunctionDefinition { Name = context.IDENTIFIER().Symbol.Text, Arguments = argsStack.Pop(), Body = stack.Pop(), Annotations = annotationSetsStack.Pop() };

        public override void ExitAnonymous_function_definition([NotNull] CalculatorDSLParser.Anonymous_function_definitionContext context)
            => ReturnValue = new YCFunctionDefinition { Name = YCFunctionDefinition.AnonymousFunctionName, Arguments = Array.Empty<string>(), Body = stack.Pop() };


        public override void ExitArgs_list_create([NotNull] CalculatorDSLParser.Args_list_createContext context)
            => argsStack.Push(new List<string> { context.IDENTIFIER().Symbol.Text });

        public override void ExitArgs_list_add([NotNull] CalculatorDSLParser.Args_list_addContext context)
            => argsStack.Peek().Add(context.IDENTIFIER().Symbol.Text);

        public override void ExitBracketed_args_list_empty([NotNull] CalculatorDSLParser.Bracketed_args_list_emptyContext context)
            => argsStack.Push(new List<string>());



        public override void ExitAnnotation_is_empty([NotNull] CalculatorDSLParser.Annotation_is_emptyContext context)
            => annotationsStack.Push((context.IDENTIFIER().Symbol.Text, YCFunctionDefinition.EmptyAnnotationValue));

        public override void ExitAnnotation_has_value([NotNull] CalculatorDSLParser.Annotation_has_valueContext context)
            => annotationsStack.Push((context.children[0].GetText(), context.children[2].GetText()));

        public override void ExitAnnotation_list_create([NotNull] CalculatorDSLParser.Annotation_list_createContext context)
        {
           var toAdd = annotationsStack.Pop(); 
           annotationSetsStack.Push(new Dictionary<string, string>{ { toAdd.Key, toAdd.Value} });
        }
        public override void ExitAnnotation_list_add([NotNull] CalculatorDSLParser.Annotation_list_addContext context)
        {
            var toAdd = annotationsStack.Pop();
            annotationSetsStack.Peek()[toAdd.Key] = toAdd.Value;
        }
        public override void ExitBracketed_annotation_list_is_empty([NotNull] CalculatorDSLParser.Bracketed_annotation_list_is_emptyContext context)
            => annotationSetsStack.Push(new());


        public override string ToString() => ""+ReturnValue;

    }



}
