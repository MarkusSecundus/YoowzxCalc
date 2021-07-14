using System;
using System.Text.RegularExpressions;

using static System.Console;

namespace MarkusSecundus.ProgrammableCalculator
{
    class Program
    {
        public static void Main()
        {
            var regex = new Regex(@"((?<f>[^a-z]*)|(?<g>[^0-9]*))");
            var matches = regex.Matches("BCDEFabcd123");
            
            foreach(var match in matches)
            {
                WriteLine(match);
            }
        }
    }
}
