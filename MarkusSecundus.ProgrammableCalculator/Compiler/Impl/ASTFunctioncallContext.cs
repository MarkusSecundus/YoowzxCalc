using MarkusSecundus.ProgrammableCalculator.Compiler.Contexts;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler.Impl
{
    class ASTFunctioncallContext<TNumber> : IASTFunctioncallContext<TNumber>
    {
        public ASTFunctioncallContext(IReadOnlyDictionary<FunctionSignature<TNumber>, Delegate> initialFunctions)
            => Functions = initialFunctions;

        public ASTFunctioncallContext() : this(CollectionsUtils.EmptyDictionary<FunctionSignature<TNumber>, Delegate>()) { }


        public IReadOnlyDictionary<FunctionSignature<TNumber>, Delegate> Functions { get; }

        private readonly DefaultValDict<FunctionSignature<TNumber>, SettableOnce<Delegate>> unresolved = new(s => new());


        public SettableOnce<Delegate> GetUnresolvedFunction(FunctionSignature<TNumber> signature)
            => unresolved[signature];



        public IASTFunctioncallContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<FunctionSignature<TNumber>, Delegate>> symbolsToBeResolved)
        {
            foreach (var symbol in symbolsToBeResolved)
                unresolved[symbol.Key].Value = symbol.Value;

            foreach (var symbol in unresolved)
                if (!symbol.Value.IsSet || symbol.Value == null)
                    throw new ArgumentException($"Symbol left unresolved: {symbol.Value}");

            var newSymbols = Functions.Chain(unresolved.Select(u => (u.Key, u.Value.Value).AsKV())).ToImmutableDictionary();

            return new ASTFunctioncallContext<TNumber>(newSymbols);
        }

        public IEnumerable<FunctionSignature<TNumber>> GetUnresolvedSymbolsList() => unresolved.Where(s => !s.Value.IsSet).Select(s=>s.Key);
    }
}
