using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Wrapper for <see cref="Action"/> that allows it to be called on <c>Dispose()</c> call.
    /// To allow for cleaner syntax with less try-finally blocks.
    /// </summary>
    public struct ActionOnDispose : IDisposable
    {
        /// <summary>
        /// Action to be performed on <c>Dispose()</c> call.
        /// </summary>
        public readonly Action ToPerform;

        /// <summary>
        /// Construct the action
        /// </summary>
        /// <param name="toPerform">Action to be performed on <c>Dispose()</c> call</param>
        public ActionOnDispose(Action toPerform) => ToPerform = toPerform;

        public void Dispose() => ToPerform?.Invoke();
    }

    /// <summary>
    /// Static class containing helper methods for concise manipulation with <see cref="ActionOnDispose"/>.
    /// Intended to be used with <c>using static..</c>
    /// </summary>
    public static class ActionOnDisposeHelper
    {
        /// <summary>
        /// Wrap an action in a Disposable decorator that calls the action automatically on its <c>Dispose()</c>.
        /// To allow for cleaner syntax with less try-finally blocks.
        /// </summary>
        /// <param name="toPerform">Action to be performed on <c>Dispose()</c> call</param>
        /// <returns>Wrapped action</returns>
        public static ActionOnDispose OnDispose(Action toPerform) => new ActionOnDispose(toPerform);
    }
}
