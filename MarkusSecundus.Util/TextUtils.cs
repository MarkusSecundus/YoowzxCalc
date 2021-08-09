using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public static class TextUtils
    {

        public static IEnumerable<string> IterateLines(this TextReader self)
        {
            for (string line; (line = self.ReadLine()) != null;)
                yield return line;
        }

        public static IEnumerable<char> IterateChars(this TextReader self)
        {
            for (int c; (c = self.Read()) != -1;)
                yield return (char)c;
        }


        public static (string Begin, string) SplitByFirstOccurence(this string self, string separator)
        {
            var i = self.IndexOf(separator);
            if (i < 0)
                return (self, "");
            return (self[..i], self[(i+1)..]);
        }
    }
}
