using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.DSL.AST;
using System.Collections.Generic;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl;

namespace MarkusSecundus.YoowzxCalc.Compiler
{
    /// <summary>
    /// Service able to process an abstract syntax tree of a computation into a function delegate natively callable from .NET runtime.
    /// </summary>
    /// <typeparam name="TNumber">Number type to be operated on</typeparam>
    public interface IYCCompiler<TNumber>
    {
        /// <summary>
        /// Compile the expression AST into runnable code.
        /// </summary>
        /// <param name="ctx">Compilation context to be used during compilation</param>
        /// <param name="toCompile">AST representing the expression to be compiled</param>
        /// <returns>Semi final compilation product that needs few final touches to turn into a runnable delegate</returns>
        /// <exception cref="System.FormatException">If the AST is not valid</exception>
        public YCCompilationResult<TNumber> Compile(IYCReadOnlyCompilationContext<TNumber> ctx, YCFunctionDefinition toCompile);

        /// <summary>
        /// Create a new instance of cannonical implementation of <see cref="IYCCompiler{TNumber}"/>
        /// </summary>
        /// <param name="op">Number operator to be used</param>
        /// <returns>New instance of a compiler for the requested number type</returns>
        public static IYCCompiler<TNumber> Make(IYCNumberOperator<TNumber> op) => YCCompilerChain.Make(op);

        /// <summary>
        /// Create a new instance of basic <see cref="IYCCompiler{TNumber}"/> implementation that does not care about function metadata or anything like that and just does the bare compilation.
        /// </summary>
        /// <param name="op">Number operator to be used</param>
        /// <returns>New instance of a compiler for the requested number type</returns>
        public static IYCCompiler<TNumber> MakeBase(IYCNumberOperator<TNumber> op) => new YCCompilerBase<TNumber>(op);

        /// <summary>
        /// Number operator used by the inner compiler implementation.
        /// </summary>
        public IYCNumberOperator<TNumber> NumberOperator { get; }


        /// <summary>
        /// Name of the annotation that specifies given function should have results cached.
        /// </summary>
        public const string CachingRequestAnnotation = "cached";
    }
}
