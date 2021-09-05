using MarkusSecundus.YoowzxCalc.Numerics;
using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc
{
    /// <summary>
    /// Canonical implementation of <see cref="IYoowzxCalculator{TNumber}"/>
    /// </summary>
    /// <typeparam name="TNumber"></typeparam>
    class YoowzxCalculator<TNumber> : IYoowzxCalculator<TNumber>
    {
        private static IYCAstBuilder DefaultAstBuilder => IYCAstBuilder.Instance;
        private static IYCCompiler<TNumber> DefaultCompiler => IYCCompiler<TNumber>.Make(YCBasicNumberOperators.Get<TNumber>());
        private static IYCFunctioncallContext<TNumber> DefaultContext => IYCFunctioncallContext<TNumber>.Make();

        private IYCAstBuilder _astBuilder;
        private IYCCompiler<TNumber> _compiler;
        private IYCFunctioncallContext<TNumber> _context;
        public IYCFunctioncallContext<TNumber> Context_impl { get => _context ??= DefaultContext; set => _context = value ; }


        public IYCAstBuilder AstBuilder { get => _astBuilder??=DefaultAstBuilder; init => _astBuilder = value; }
        public IYCCompiler<TNumber> Compiler { get => _compiler??=DefaultCompiler; init => _compiler = value; }
        public IYCInterpretationContext<TNumber> Context { get => Context_impl; init => Context_impl = (value is IYCFunctioncallContext<TNumber> c)?c: IYCFunctioncallContext<TNumber>.Make().ResolveSymbols((value??DefaultContext).Functions); }


        
        public Delegate Get(YCFunctionSignature<TNumber> signature)
        {
            return Context.Functions[signature];
        }

        TDelegate Get<TDelegate>(string name) where TDelegate: Delegate
        {
            return (TDelegate)Get(YCCompilerUtils.GetDelegateTypeSignature<TNumber, TDelegate>(name));
        }


        TDelegate Compile<TDelegate>(string function) where TDelegate : Delegate
        {
            var tree = AstBuilder.Build(function);
            var result = Compiler.Compile(_context, tree);
            return result.Compile<TDelegate>();
        }


        public IYoowzxCalculator<TNumber> AddFunctions(IEnumerable<string> toAdd)
        {
            foreach(var function in toAdd)
            {
                var tree = AstBuilder.Build(function);
                var signature = tree.GetSignature<TNumber>();
                var result = Compiler.Compile(Context_impl, tree).Compile();
                Context_impl.GetUnresolvedFunction(signature).Value = result;
            }
            Context_impl.ResolveSymbols();

            return this;
        }


        public IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd, out YCFunctionSignature<TNumber> signature) where TDelegate : Delegate
        {
            signature = toAdd.GetSignature<TNumber, TDelegate>(name);

            Context_impl = Context_impl.ResolveSymbols((signature, toAdd));
            
            return this;
        }
        
        public IYoowzxCalculator<TNumber> AddFunction(string expression, out YCFunctionSignature<TNumber> signature, out Delegate result)
        {
            var tree = AstBuilder.Build(expression);
            signature = tree.GetSignature<TNumber>();
            result = Compiler.Compile(Context_impl, tree).Compile();

            Context_impl = Context_impl.ResolveSymbols((signature, result));

            return this;
        }

        TDelegate IYoowzxCalculator<TNumber>.Compile<TDelegate>(string function) => Compile<TDelegate>(function);
        TDelegate IYoowzxCalculator<TNumber>.Get<TDelegate>(string signature) => Get<TDelegate>(signature);

    }
}
