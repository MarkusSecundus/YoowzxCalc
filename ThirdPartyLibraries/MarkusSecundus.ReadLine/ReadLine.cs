using Internal.ReadLine;
using Internal.ReadLine.Abstractions;

using System.Collections.Generic;

namespace System
{

    /// <summary>
    /// Custom fork of <see href="https://github.com/tonerdo/readline"/>
    /// with added support of EOF simulation through Ctrl+D (like on Unix)
    /// and stdin redirection.
    /// 
    /// All credits go to the original creator: Toni Solarin-Sodara
    /// </summary>
    public static class ReadLine
    {
        private static List<string> _history;

        static ReadLine()
        {
            _history = new List<string>();
        }

        public static void AddHistory(params string[] text) => _history.AddRange(text);
        public static List<string> GetHistory() => _history;
        public static void ClearHistory() => _history = new List<string>();
        public static bool HistoryEnabled { get; set; }
        public static IAutoCompleteHandler AutoCompletionHandler { private get; set; }

        public static string Read(string prompt = "", string @default = null)
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2(), _history, AutoCompletionHandler);
            string text = GetText(keyHandler);

            if (String.IsNullOrWhiteSpace(text) && !String.IsNullOrWhiteSpace(@default))
            {
                text = @default;
            }
            else
            {
                if (HistoryEnabled)
                    _history.Add(text);
            }

            return text;
        }

        public static string ReadPassword(string prompt = "")
        {
            Console.Write(prompt);
            KeyHandler keyHandler = new KeyHandler(new Console2() { PasswordMode = true }, null, null);
            return GetText(keyHandler);
        }


        private static string GetText(KeyHandler keyHandler)
        {
            ConsoleKeyInfo keyInfo = ReadKey();
            //<Customised>
            if (keyInfo.IsEOF() )
            {
                return null;
            }
            //</Customised>

            while (keyInfo.Key != ConsoleKey.Enter)
            {
                keyHandler.Handle(keyInfo);
                keyInfo = ReadKey();
            }

            Console.WriteLine();
            return keyHandler.Text;
        }


        //<Customised>
        private static bool _HasConsole = true;
        private static ConsoleKeyInfo ReadKey()
        {
            if (_HasConsole)
            {
                try
                {
                    return Console.ReadKey(true);
                }
                catch { _HasConsole = false; }
            }
            int c = Console.In.Read();
            return new ConsoleKeyInfo((char)c, c=='\n'? ConsoleKey.Enter : ConsoleKey.D, false, false, control: c == -1);
        }

        private static bool IsEOF(this ConsoleKeyInfo key)
            => key.Key == ConsoleKey.D && (key.Modifiers & ConsoleModifiers.Control) != 0;
        //</Customised>
    }
}
