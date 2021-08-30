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
    public class CmdOptions
    {
        [Option('f', "file", Required = false, HelpText = "Files from which to load definitions", Separator =';', Max =int.MaxValue)]
        public IEnumerable<string> FilesToLoad { get; init; }

        [Option('e', "eval", Required = false, HelpText ="Provide an expression to evaluate directly")]
        public string Eval { get; init; }
    }

    public class CmdMain
    {
        private IYoowzxCalculator<num> Calc;
        private IYCNumberOperator<num> Operator;

        public delegate string CmdCommand(string args);

        private readonly Dictionary<string, CmdCommand> Commands;

        private readonly TextWriter Out;

        private readonly string Prompt;

        public CmdMain(TextWriter output=null, string prompt = null)
        {
            Prompt = prompt ?? ">>> ";
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


        public void Repl()
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

        public void RunCommand(string command)
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
            foreach (var line in rdr.IterateLines())
                Calc.AddFunction(line, out _, out _);

            return null;
        }

        string Save(string path)
        {
            using var wrt = new StreamWriter(path.Trim());
            foreach (var expr in _definitionsHistory)
                wrt.WriteLine(expr);

            return null;
        }
        string List(string _=null)
        {
            return _definitionsHistory.MakeString("\n");
        }

        string Help(string _ = null) => HelpString;

        string Exit(string args = null)
        {
            Environment.Exit(0);
            return null;
        }




        public static void Main(string[] args) => Launch(args);
        
        public static void Launch(string[] args) => Parser.Default.ParseArguments<CmdOptions>(args).WithParsed(new CmdMain().Main);




        public static string HelpString =>
@"Yoowzx Calc v1.0

Commands:
- help ... Print this overview

- exit ... Exit this application

- load [filepath] ... Load definitions from the specified file

- save [filepath] ... Save all function defined so far in this session of Yoowzx Calc to specified file

- list (all | defined) ... List all functions that were defined so far in this session of Yoowzx Calc

- eval? [expression or function definition] ... Evaluate a mathematical expression
    


" + new string('_', Console.WindowWidth-1)+
@"

Author: Jakub Hroník
git: https://github.com/MarkusSecundus/YoowzxCalc";

    }
}
