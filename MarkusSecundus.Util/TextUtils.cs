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

    }
}
