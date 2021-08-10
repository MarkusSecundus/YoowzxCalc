using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCFunctioncallContext<TNumber> : IYCFunctioncallContext<TNumber>
    {
        public YCFunctioncallContext(IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> initialFunctions)
            => Functions = initialFunctions;

        public YCFunctioncallContext() : this(CollectionsUtils.EmptyDictionary<YCFunctionSignature<TNumber>, Delegate>()) { }


        public IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> Functions { get; }

        private readonly DefaultValDict<YCFunctionSignature<TNumber>, SettableOnce<Delegate>> unresolved = new(s => new());


        public SettableOnce<Delegate> GetUnresolvedFunction(YCFunctionSignature<TNumber> signature)
            => unresolved[signature];



        public IYCFunctioncallContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<YCFunctionSignature<TNumber>, Delegate>> symbolsToBeResolved)
        {
            foreach (var symbol in symbolsToBeResolved)
                unresolved[symbol.Key].Value = symbol.Value;

            foreach (var symbol in unresolved)
                if (!symbol.Value.IsSet || symbol.Value == null)
                    throw new ArgumentException($"Symbol left unresolved: {symbol.Value}");

            var bld = ImmutableDictionary.CreateBuilder<YCFunctionSignature<TNumber>, Delegate>();
            foreach (var f in Functions) bld[f.Key] = f.Value;
            foreach (var f in unresolved) bld[f.Key] = f.Value.Value;

            return new YCFunctioncallContext<TNumber>(bld.ToImmutable());
        }

        public IEnumerable<YCFunctionSignature<TNumber>> GetUnresolvedSymbolsList() => unresolved.Where(s => !s.Value.IsSet).Select(s => s.Key);
    }
}
