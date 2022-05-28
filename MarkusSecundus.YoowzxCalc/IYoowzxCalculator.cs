using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using MarkusSecundus.YoowzxCalc.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc
{
    /// <summary>
    /// Facade covering the Yoowzx pipeline for mathematical expressions evaluation.
    /// <para/>
    /// Provides a convenient and straightforward way to compile a text representated function directly to executable code etc.
    /// </summary>
    /// <typeparam name="TNumber">Numeric type used in the calculations</typeparam>
    public interface IYoowzxCalculator<TNumber>
    {
        /// <summary>
        /// Object responsible for transformation of the text-written expression to AST
        /// </summary>
        public IYCAstBuilder AstBuilder { get; init; }
        /// <summary>
        /// Object responsible for compilation of the AST to executable code
        /// </summary>
        public IYCCompiler<TNumber> Compiler { get; init; }
        /// <summary>
        /// Object carrying function definitions that can be referenced from the expression being compiled
        /// </summary>
        public IYCReadOnlyCompilationContext<TNumber> Context { get; init; }



        /// <summary>
        /// Gets an existing function with given signature from the Context.
        /// </summary>
        /// <param name="signature">Signature of the sought function</param>
        /// <returns>Function residing in the context under the provided signature.</returns>
        /// <exception cref="KeyNotFoundException">If no function is defined for the provided signature.</exception>
        public Delegate Get(YCFunctionSignature<TNumber> signature);
        /// <summary>
        /// Gets an existing function with given signature from the Context.
        /// Signature arguments are deduced from the provided Delegate type.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the sought function. Must be concrete - <see cref="Delegate"/> and <see cref="MulticastDelegate"/> are not allowed.</typeparam>
        /// <param name="signature">Name of the sought function</param>
        /// <returns>Function residing in the context under the provided signature.</returns>
        /// <exception cref="KeyNotFoundException">If no function is defined for the provided signature.</exception>
        /// <exception cref="ArgumentException">If <typeparamref name="TDelegate"/> is exactly of type <see cref="Delegate"/> or <see cref="MulticastDelegate"/></exception>
        public TDelegate Get<TDelegate>(string signature) where TDelegate : Delegate;

        

        /// <summary>
        /// Compiles a text-written expression to executable function.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the result delegate</typeparam>
        /// <param name="function">Expression to be compiled</param>
        /// <returns>Compiled result</returns>
        public TDelegate Compile<TDelegate>(string function) where TDelegate : Delegate;

        /// <summary>
        /// Compiles all the given expressions and adds them one after the other to the Context.
        /// </summary>
        /// <param name="toAdd">List of text-represented expressions</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public IYoowzxCalculator<TNumber> AddFunctions(IEnumerable<string> toAdd);

        /// <summary>
        /// Compiles given expression and adds it to the Context.
        /// </summary>
        /// <param name="expression">Text-represented expression</param>
        /// <param name="signature">Signature describing the compiled expression, under which it has just been added to the Context</param>
        /// <param name="result">Executable product of the compilation that has just been added to the Context</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public IYoowzxCalculator<TNumber> AddFunction(string expression, out YCFunctionSignature<TNumber> signature, out Delegate result);


        /// <summary>
        /// Adds a delegate to Context.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate being added. Useful to specify it for convenient passing of inline lambdas</typeparam>
        /// <param name="name">Name under which the function is to be added.</param>
        /// <param name="toAdd">Delegate to be added</param>
        /// <param name="signature">Signature under which the delegate has just been added to Context. Number of arguments was deduced from the header of the function that the delegate points to.</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd, out YCFunctionSignature<TNumber> signature) where TDelegate: Delegate;

        /// <summary>
        /// Adds a delegate to Context.
        /// <para/>
        /// Additional overload for more better user comfort - alias for <c>AddFunction(name, toAdd, out _)</c>
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate being added. Useful to specify it for convenient passing of inline lambdas</typeparam>
        /// <param name="name">Name under which the function is to be added.</param>
        /// <param name="toAdd">Delegate to be added</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd) where TDelegate : Delegate
            => AddFunction(name, toAdd, out _);



        /// <summary>
        /// Creates new instance of the cannonical implementation of the calculator.
        /// </summary>
        /// <param name="astBuilder">Instance of AstBuilder. If null, a new instance of cannonical implementation will be created.</param>
        /// <param name="compiler">Instance of compiler. If null, a new instance of cannonical implementation will be created with NumberOperated obtained from <c><see cref="YCBasicNumberOperators.Get{TNumber}()"/></c>.</param>
        /// <param name="context">Instance of kontextu. If null, a new instance of cannonical implementation will be created.</param>
        /// <returns>New instance of calculator's cannonical implementation</returns>
        public static IYoowzxCalculator<TNumber> Make(IYCAstBuilder astBuilder = null, IYCCompiler<TNumber> compiler = null, IYCReadOnlyCompilationContext<TNumber> context = null)
            => new YoowzxCalculator<TNumber>() { AstBuilder = astBuilder, Compiler = compiler, Context = context };
    }




    /// <summary>
    /// Static class with extension methods for more comfortable work with <see cref="IYoowzxCalculator{TNumber}"/>
    /// </summary>
    public static class YoowzxCalculatorExtensions
    {
        /// <summary>
        /// Compiles all the given expressions and adds them one after the other to the Context.
        /// </summary>
        /// Additional overload for more better user comfort.
        /// </summary>
        /// <param name="toAdd">List of text-represented expressions</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public static IYoowzxCalculator<TNumber> AddFunctions<TNumber>(this IYoowzxCalculator<TNumber> self, params string[] toAdd)
            => self.AddFunctions(toAdd);

        /// <summary>
        /// Adds a delegate to Context.
        /// <para/>
        /// Additional overload for more better user comfort.
        /// </summary>
        /// <param name="name">Name under which the function is to be added.</param>
        /// <param name="toAdd">Delegate to be added</param>
        /// <returns><c>this</c> for chaining purposes</returns>
        public static IYoowzxCalculator<TNumber> AddFunction<TNumber>(this IYoowzxCalculator<TNumber> self, string name, Delegate toAdd)
            => self.AddFunction(name, toAdd);

    }
}
