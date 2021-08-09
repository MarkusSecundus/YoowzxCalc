using CommandLine;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
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


    public class GraphDrawer
    {
        public class DrawerOptions
        {

        }
    }

    public class CmdMain
    {
        private IYoowzxCalculator<num> Calc = IYoowzxCalculator<num>.Make();

        public delegate string CmdCommand(string args);

        private readonly Dictionary<string, CmdCommand> Commands;

        private readonly TextWriter Out;
        private readonly TextReader In;

        public CmdMain(TextReader input=null, TextWriter output=null)
        {
            In = input ?? Console.In;
            Out = output ?? Console.Out;
            Commands = new()
            {
                ["eval"] = Eval,
                ["load"] = Load,
                ["graph"] = Graph,
                ["exit"] = Exit
            };
        }



        public void Main(CmdOptions args)
        {
            args.FilesToLoad.ForAll(Load);

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
            foreach(var line in In.IterateLines())
            {
                try
                {
                    RunCommand(line);
                }catch(Exception e)
                {
                    Out.WriteLine("! {0}", e.Message);
                }
            }
            Exit();
        }

        public void RunCommand(string command)
        {
            command = command.TrimStart();
            if (string.IsNullOrEmpty(command)) return;

            var (commandName, args) = command.SplitByFirstOccurence(" ");
            args.Trim();

            var output = Commands.TryGetValue(commandName, out var cmd)
                                ? cmd(args)
                                : Eval(command);
            if (output != null)
                Out.WriteLine(output);
        }

        string Eval(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return null;

            Calc.AddFunction(expression, out var signature, out var result);
            if (signature.IsAnonymousExpression())
                return "" + ((Func<num>)result)();
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

        string Graph(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return null;

            var f = Calc.Compile<Func<num, num>>(expression);

            throw new NotImplementedException("Graph functionality not yet implemented");

            //return null;
        }

        string Exit(string args = null)
        {
            Out.WriteLine("*** YoowzxCalc - Exiting! ***");

            Environment.Exit(0);

            return null;
        }

        public static void Main(string[] args) => Parser.Default.ParseArguments<CmdOptions>(args).WithParsed(new CmdMain().Main);
    }
}
