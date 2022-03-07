using System;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    /// <summary>
    /// Raw output of a compilation that still needs some finishing touches to turn into a fully functional delegate.
    /// </summary>
    /// <typeparam name="TNumber">Number type operated on</typeparam>
    public interface IYCCompilationResult<TNumber>
    {
    {
        /// <summary>
        /// Finalize into a weakly typed delegate
        /// </summary>
        /// <returns>Weakly typed final product of the compilation</returns>
        public Delegate Finalize() => Finalize<Delegate>();

        /// <summary>
        /// Finalize into a strongly typed delegate of specified type
        /// </summary>
        /// <typeparam name="TDelegate">Type of the result delegate</typeparam>
        /// <returns>Strongly typed final product of the compilation</returns>
        /// <exception cref="InvalidCastException">If the compilation result cannot be cast to the specified type</exception>
        public TDelegate Finalize<TDelegate>() where TDelegate : Delegate;
    }
}
