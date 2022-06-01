using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Static class with utility functions that should have been in standard library.
    /// </summary>
    public static class TextUtils
    {
        /// <summary>
        /// Generator that iterates through all lines in the text stream until it is depleted
        /// </summary>
        /// <param name="self">Text stream to be iterated</param>
        /// <returns>Stream of lines as returned by TextReader::ReadLine()</returns>
        public static IEnumerable<string> IterateLines(this TextReader self)
        {
            for (string line; (line = self.ReadLine()) != null;)
                yield return line;
        }

        /// <summary>
        /// Generator that iterates through all chars in the text stream until it is depleted
        /// </summary>
        /// <param name="self">Text stream to be iterated</param>
        /// <returns>Stream of chars</returns>
        public static IEnumerable<char> IterateChars(this TextReader self)
        {
            for (int c; (c = self.Read()) != -1;)
                yield return (char)c;
        }


        /// <summary>
        /// Splits a string in two parts by the first occurence of specified symbol
        /// </summary>
        /// <param name="self">The string to be split</param>
        /// <param name="separator">The delimiter according to which the splitting will be done</param>
        /// <returns>
        /// <code>(self, null)</code> if the separator is not found, otherwise the two halves, beth excluding the separator sequence
        /// </returns>
        public static (string Begin, string) SplitByFirstOccurence(this string self, string separator)
        {
            var i = self.IndexOf(separator);
            if (i < 0)
                return (self, null);
            return (self[..i], self[(i+separator.Length)..]);
        }





        private delegate (int, char) EscapeResolver(ReadOnlySpan<char> substring);
        private static Dictionary<char, EscapeResolver> Escapers = new()
        {
            ['a'] = s => (0, '\a'),
            ['b'] = s => (0, '\b'),
            ['f'] = s => (0, '\f'),
            ['n'] = s => (0, '\n'),
            ['r'] = s => (0, '\r'),
            ['t'] = s => (0, '\t'),
            ['v'] = s => (0, '\v'),
            ['\\'] = s => (0, '\\'),
            ['o'] = s => (2, convertOctal(s)),
            ['O'] = s => (2, convertOctal(s)),
            ['x'] = s => convertHex(s),
            ['X'] = s => convertHex(s)
        };

        private static char convertOctal(ReadOnlySpan<char> substring)
        {
            if (substring.Length < 3) throw new FormatException($"Invalid escape sequence: '\\{new string(substring)}'");
            return (char)Convert.ToInt16(new string(substring.Slice(1, 2)), 8);
        }
        private static (int, char) convertHex(ReadOnlySpan<char> substring)
        {
            if(substring.Length < 3) throw new FormatException($"Invalid escape sequence: '\\{new string(substring)}'");
            for(int t = 5; t >= 1; --t)
            {
                try 
                {
                    return (t, (char)Convert.ToInt32(new string(substring.Slice(1, t)), 16));
                }
                catch { }
            }
            throw new FormatException($"Invalid escape sequence: '\\{new string(substring.Slice(0, 3))}'");
        }

        //TODO: desperately needs a refactor!
        /// <summary>
        /// Gets a raw string like what would be a part of C# code and replaces all escape sequences in it by the characters they represent.
        /// Behaves according to the specification C# uses (<see href="https://docs.microsoft.com/en-us/cpp/c-language/escape-sequences"/>)
        /// except quotes and double-quotes are not considered escapable chars.
        /// </summary>
        /// <param name="self">Raw string containing escape sequences to be resolved</param>
        /// <returns></returns>
        /// <exception cref="FormatException">When an invalid escape sequence is found</exception>
        public static string ResolveEscapeSequences(this string self)
        {
            var bld = new StringBuilder();

            for(int i =0; i< self.Length; ++i)
            {
                char c = self[i];
                if (c != '\\')
                    bld.Append(c);
                else
                {
                    ++i;
                    if (i >= self.Length) throw new FormatException($"Invalid escape sequence: '{self.Substring(i-1)}'");
                    try
                    {
                        var toAppend = Escapers[self[i]](self.AsSpan(i));
                        bld.Append(toAppend.Item2);
                        i += toAppend.Item1;
                    }
                    catch
                    {
                        throw new FormatException($"Invalid escape sequence: '\\{self[i]}'");
                    }
                }
            }

            return bld.ToString();
        }
    }
}
