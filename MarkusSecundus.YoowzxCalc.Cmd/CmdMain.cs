using CommandLine;
using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using num = System.Double;


namespace MarkusSecundus.YoowzxCalc.Cmd
{
    /// <summary>
    /// Data class specifying command line arguments that can be passed to the program.
    /// </summary>
    public class CmdOptions
    {
        /// <summary>
        /// List of files containing definitions that are supposed to be loaded before the program starts.
        /// </summary>
        [Option('f', "file", Required = false, HelpText = "Files from which to load definitions", Separator =';', Max =int.MaxValue)]
        public IEnumerable<string> FilesToLoad { get; init; }

        /// <summary>
        /// Expression to be evaluated. If non-null, no REPL will be run, instead the result will just be written to stdout.
        /// </summary>
        [Option('e', "eval", Required = false, HelpText ="Provide an expression to evaluate directly")]
        public string Eval { get; init; }
    }

    /// <summary>
    /// Main class responsible for operation of the commandline calculator.
    /// </summary>
    public class CmdMain
    {
        private IYoowzxCalculator<num> Calc;
        private IYCNumberOperator<num> Operator;

        private delegate string CmdCommand(string args);

        private readonly Dictionary<string, CmdCommand> Commands;

        private readonly TextWriter Out;

        private readonly string Prompt;

        /// <summary>
        /// Subclass carrying constants
        /// </summary>
        public static class Const
        {
            /// <summary>
            /// Prompt that will be written by default by REPL when user input is being expected
            /// </summary>
            public const string DefaultPrompt = ">>> ";
        }

        /// <summary>
        /// Initializes the caluculator instance by specified text input.
        /// </summary>
        /// <param name="output">Text stream to which the output shall be written (by default <see cref="System.Console.Out"/>).</param>
        /// <param name="prompt">Prompt to be printed when requesting user input (by default <see cref="CmdMain.Const.DefaultPrompt"/>).</param>
        public CmdMain(TextWriter output=null, string prompt = null)
        {
            Prompt = prompt ?? Const.DefaultPrompt;
            Out = output ?? Console.Out;
            Commands = new()
            {
                ["eval"] = Eval,
                ["load"] = Load,
                ["save"] = Save,
                ["list"] = List,
                ["exit"] = Exit,
                ["help"] = Help,
            };

            Operator = YCBasicNumberOperators.Get<num>();
            Calc = IYoowzxCalculator<num>.Make(compiler: IYCCompiler<num>.Make(Operator));
        }

        /// <summary>
        /// Runs the program.
        /// <para/>
        /// Depending on the arguments either evaluates an expression and exits or begins a REPL loop.
        /// </summary>
        /// <param name="args">List of arguments</param>
        public void Main(CmdOptions args)
        {
            foreach(var f in args.FilesToLoad) Load(f);

            if (args.Eval != null)
            {
                RunCommand(args.Eval);
            }
            else
            {
                Repl();
            }
        }


        void Repl()
        {
            for(string line; (line = ReadLine.Read(Prompt))!= null;)
            {
                try
                {
                    ReadLine.AddHistory(line);
                    RunCommand(line);
                }catch(Exception e)
                {
                    Out.WriteLine("Error: {0} - {1}", e.GetType(), e.Message);
                }
            }
            Exit();
        }

        void RunCommand(string command)
        {
            command = command.TrimStart();
            if (string.IsNullOrEmpty(command)) return;

            var (commandName, args) = command.SplitByFirstOccurence(" ");
            args = args?.Trim()??"";

            var output = Commands.TryGetValue(commandName, out var cmd)
                                ? cmd(args)
                                : Eval(command);
            if (output != null)
                Out.WriteLine(output);
        }

        private List<string> _definitionsHistory = new();
        string Eval(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return null;
            Calc.AddFunction(expression, out var signature, out var result);

            if (!signature.IsAnonymousExpression())
                _definitionsHistory.Add(expression);
            if (signature.ArgumentsCount == 0)
            {
                var f = (Func<num>)result;
                return "" + f();
            }

            return null;
        }

        string Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            using var rdr = new StreamReader(path);

            Calc.AddFunctions(rdr.IterateLines().Where(s=>!string.IsNullOrWhiteSpace(s)));

            return null;
        }

        string Save(string path)
        {
            using var wrt = new StreamWriter(path.Trim());
            foreach (var expr in _definitionsHistory)
                wrt.WriteLine(expr);

            return null;
        }

        string List(string arg=null)
        {
            switch( arg = arg.Trim() )
            {
                case "":
                    return Operator.StandardLibrary.Keys.Chain(Calc.Context.Functions.Keys)
                                .Distinct().Where(f=>!f.IsAnonymousExpression())
                                .Select(f => f.ToStringTypeless()).MakeString("\n");

                case "save":
                    return _definitionsHistory.MakeString("\n");

                default:
                    throw new ArgumentException($"Invalid mode: '{arg}' - must be one of ( '' | 'all' )");
            }
        }



        string Help(string _ = null) => HelpString;

        string Exit(string args = null)
        {
            Environment.Exit(0);
            return null;
        }



        
        public static void Main(string[] args) => Parser.Default.ParseArguments<CmdOptions>(args).WithParsed(new CmdMain().Main);




        public static string HelpString =>
@"Yoowzx Calc v0.1

Commands:
- help ... Prints this overview

- exit ... Exits the program

- load [filepath] ... Loads definitions from given file

- save [filepath] ... Saves all the functions defined in this session to the given file. (Use `list save` for preview of what exactly will be saved)

- list (save) ... Print the functions that are currently available / that would be saved by the `save` command

- (eval) [expression or function definition] ... Evaluate mathematical expression
    


" + new string('_', Console.WindowWidth-1)+
@"

Author: Jakub Hroník
git: https://github.com/MarkusSecundus/YoowzxCalc";

    }
}
