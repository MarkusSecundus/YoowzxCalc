using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public static class ExceptionUtils
    {
        public static TException Aggregate<TException>(this IReadOnlyCollection<TException> self) where TException : Exception
        {
            if (self.Count == 1) return self.First();
            var aggr = new AggregateException(self);
            return ExceptionConstructorContainer<TException>.WithMessageAndInner(aggr.Message, aggr);
        }

        private static class ExceptionConstructorContainer<TException>
        {
            public readonly static Func<string, Exception, TException> WithMessageAndInner 
                = (Func<string, Exception, TException>) typeof(TException).GetConstructor(new[] { typeof(string), typeof(Exception) }).MakeDelegate();

        }
    }
}
