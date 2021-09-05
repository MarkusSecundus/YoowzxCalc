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
    /// Datová třída specifikující argumenty příkazové řádky, jež mohou programu být předány.
    /// </summary>
    public class CmdOptions
    {
        /// <summary>
        /// Seznam souborů s definicemi, jež mají před spuštěním programu být načteny.
        /// </summary>
        [Option('f', "file", Required = false, HelpText = "Files from which to load definitions", Separator =';', Max =int.MaxValue)]
        public IEnumerable<string> FilesToLoad { get; init; }

        /// <summary>
        /// Výraz k přímému vyhodnocení. Pokud je ne-null, nebude spuštěn REPL, místo toho bude pouze vyhodnocen daný výraz a vypsán výsledek.
        /// </summary>
        [Option('e', "eval", Required = false, HelpText ="Provide an expression to evaluate directly")]
        public string Eval { get; init; }
    }

    /// <summary>
    /// Hlavní třída kontrolující běh terminálového kalkulátoru.
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
        /// Podtřída nesoucí konstanty.
        /// </summary>
        public static class Const
        {
            /// <summary>
            /// Výchozí 
            /// </summary>
            public const string DefaultPrompt = ">>> ";
        }

        /// <summary>
        /// Inicializuje instanci kalkulátoru specifikovaným textovým výstupem.
        /// </summary>
        /// <param name="output">Textový proud do nějž bude zapisován výstup kalkulátoru.</param>
        /// <param name="prompt">Výzva jež se vždy vypíše na začátku řádky očekávající uživatelský vstup.</param>
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
        /// Provede jeden průběh aplikace.
        /// <para/>
        /// V závislosti na předaných argumentech buď vyhodnotí výraz a skončí, nebo započne REPL smyčku.
        /// </summary>
        /// <param name="args">List argumentů</param>
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

Příkazy:
- help ... Vypíše tento přehled

- exit ... Ukončí tento program

- load [filepath] ... Načte definice z daného souboru

- save [filepath] ... Uloží všechny v této relaci definované funkce do specifikovaného souboru. (Zavolej `list save` pro náhled funkcí, jež budou uloženy) 

- list (save) ... Vypiš funkce, jež jsou k dispozici / jež by byly uloženy příkazem `save`

- (eval) [expression or function definition] ... Vyhodnoť matematický výraz (gramatiku viz níže)
    


" + new string('_', Console.WindowWidth-1)+
@"

Autor: Jakub Hroník
git: https://github.com/MarkusSecundus/YoowzxCalc";

    }
}
