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


        public static string Repeat(this string self, int times)
        {
            var bld = new StringBuilder(self.Length * times + 1);

            while (--times >= 0)
                bld.Append(self);

            return bld.ToString();
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


        /// <summary>
        /// Clamps the string to specified length, cutting the left part if necessary.
        /// </summary>
        /// <param name="self">String to clamp</param>
        /// <param name="maxLength">Max length of the clamped string</param>
        /// <returns>String clamped to specified length</returns>
        public static string ClampToLength(this string self, int maxLength)
            => self.Length <= maxLength ? self : self.Substring(0,maxLength);


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
                char c() => self[i];
                if (c() != '\\')
                    bld.Append(c());
                else
                {
                    ++i;
                    if (i >= self.Length) throw invalid();

                    var toAppend =  c() switch
                    {
                        'a' => '\a',
                        'b' => '\b',
                        'f' => '\f',
                        'n' => '\n',
                        'r' => '\r',
                        't' => '\t',
                        'v' => '\v',
                        '\\' => '\\',
                        'o' => convertNum(8, 2),
                        'O' => convertNum(8, 2),
                        'x' => convertNum(16, 5),
                        'X' => convertNum(16, 5),
                        _ => throw invalid(2)
                    };

                    bld.Append(toAppend);
                }

                char convertNum(int numericBase, int maxLength, int minLength=1)
                {
                    for (int t = maxLength; t >= minLength; --t)
                    {
                        try
                        {
                            var ret = (char)Convert.ToInt64(substr(1, t), numericBase);
                            i += t;
                            return ret;
                        }
                        catch { }
                    }
                    throw invalid(4);
                }
                string substr(int begin, int len) => self.Substring(i+begin, len);
                FormatException invalid(int maxLength=int.MaxValue) => new FormatException($"Invalid escape sequence: '{self.Substring(i - 1).ClampToLength(maxLength)}'");
            }

            return bld.ToString();
        }
    }
}
