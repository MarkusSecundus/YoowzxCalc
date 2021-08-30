using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Static class with utility functions that should have been in standard library.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Creates an exception object of specified type by aggregating multiple exceptions of that same type.
        /// 
        /// Assumes that TException has public constructor with signature (string message, System.Exception innerException)
        /// </summary>
        /// <typeparam name="TException">Type of the result exception</typeparam>
        /// <param name="self">List of exceptions to be aggregated</param>
        /// <returns>The original exception if there is exactly one of it, otherwise an aggregate of all the provided exceptions.</returns>
        public static TException Aggregate<TException>(this IEnumerable<TException> self) where TException : Exception
        {
            var aggr = new AggregateException(self);
            if (aggr.InnerExceptions.Count == 1) return (TException)aggr.InnerExceptions[0];
            return ExceptionConstructorContainer_ArgsAreMessageAndInner<TException>.Value(aggr.Message, aggr);
        }

        private static class ExceptionConstructorContainer_ArgsAreMessageAndInner<TException>
        {
            public readonly static Func<string, Exception, TException> Value 
                = (Func<string, Exception, TException>) typeof(TException).GetConstructor(new[] { typeof(string), typeof(Exception) }).MakeDelegate();

        }
    }
}
